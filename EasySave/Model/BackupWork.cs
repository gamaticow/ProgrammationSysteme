using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Priority_Queue;

namespace EasySave.Model
{
    class BackupWork : IObservable<BackupLog>, IObservable<BackupState>, IObservable<string>
    {
        private List<IObserver<BackupLog>> logObservers = new List<IObserver<BackupLog>>();
        private List<IObserver<BackupState>> stateObservers = new List<IObserver<BackupState>>();
        private List<IObserver<string>> saveObservers = new List<IObserver<string>>();

        public int Id { get; private set; }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                Save();
                if(Model.Instance.SocketServer != null)
                    Model.Instance.SocketServer.RenameBackupWork(this);
            }
        }
        private string _sourceDirectory;
        public string sourceDirectory
        {
            get
            {
                return _sourceDirectory;
            }
            set
            {
                _sourceDirectory = value;
                Save();
            }
        }
        
        private string _targetDirectory;
        public string targetDirectory
        {
            get
            {
                return _targetDirectory;
            }
            set
            {
                _targetDirectory = value;
                Save();
            }
        }

        private BackupType _backupType;
        public BackupType backupType
        {
            get
            {
                return _backupType;
            }
            set
            {
                _backupType = value;
                Save();
            }
        }
        public BackupStateEnum State { get; private set; }

        private static Semaphore HugeFiles = new Semaphore(1, 1);
        private Semaphore pause = new Semaphore(1, 1);
        private Thread thread;
        private bool interrupt = false;

        // Datas fo backup state
        private string sourceFilePath;
        private string targetFilePath;
        private int totalFilesToCopy;
        private long totalFilesSize;
        private int nbFilesLeftToDo;
        private bool forceProgress;

        public BackupWork(string name, string sourceDirectory, string targetDirectory, BackupType type)
        {
            lock (Model.Instance.backupIdLock)
            {
                this.Id = Model.Instance.BackupId++;
            }
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
            this.backupType = type;

            State = BackupStateEnum.END;
            sourceFilePath = "";
            targetFilePath = "";
            totalFilesToCopy = 0;
            totalFilesSize = 0;
            nbFilesLeftToDo = 0;
            forceProgress = false;
        }

        public BackupWork(int id, string name, string sourceDirectory, string targetDirectory, BackupType type)
        {
            this.Id = id;
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
            this.backupType = type;

            State = BackupStateEnum.END;
            sourceFilePath = "";
            targetFilePath = "";
            totalFilesToCopy = 0;
            totalFilesSize = 0;
            nbFilesLeftToDo = 0;
            forceProgress = false;
        }

        public void ExecuteBackup()
        {
            if ((State == BackupStateEnum.END || State == BackupStateEnum.INTERRUPTED) && ((thread != null && thread.ThreadState == System.Threading.ThreadState.Stopped) || thread == null))
            {
                interrupt = false;
                thread = new Thread(ExecuteBackupWork);
                thread.Name = name;
                thread.Start();
            }
            else if (State == BackupStateEnum.PAUSE)
            {
                pause.Release();
            }
        }

        public void Pause()
        {
            if (State == BackupStateEnum.ACTIVE && thread != null && thread.ThreadState != System.Threading.ThreadState.Stopped)
            {
                Task.Run(() =>
                {
                    pause.WaitOne();
                    Thread.Sleep(50);
                    if (State == BackupStateEnum.ACTIVE)
                        State = BackupStateEnum.PAUSE;
                    UpdateState();
                });
            }
        }

        public void Interupt()
        {
            if (State == BackupStateEnum.ACTIVE && thread != null && thread.ThreadState != System.Threading.ThreadState.Stopped)
            {
                interrupt = true;
            }
            else if (State == BackupStateEnum.PAUSE && thread != null && thread.ThreadState != System.Threading.ThreadState.Stopped)
            {
                interrupt = true;
                pause.Release();
            }
        }

        protected void ExecuteBackupWork()
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);

            if (IsBusinessAppRunning() == null)
            {
                List<BackupFile> allFiles = new List<BackupFile>();
                // Execute GetFiles() and get the total file size that will be write in the state JSON file
                long totalFileSize = GetFiles(allFiles, source, target);
                if (totalFileSize < 0)
                {
                    return;
                }
                totalFilesSize = totalFileSize;

                SimplePriorityQueue<BackupFile> files = new SimplePriorityQueue<BackupFile>();
                SimplePriorityQueue<BackupFile> hugeFiles = new SimplePriorityQueue<BackupFile>();

                foreach (BackupFile file in allFiles)
                {
                    files.Enqueue(file, GetFilePriority(file.source.FullName));
                }

                // Get the current file size that will be write in the state JSON file
                totalFilesToCopy = files.Count;
                nbFilesLeftToDo = files.Count;

                // Loop on each file we need to save 
                while (files.Count > 0 || hugeFiles.Count > 0)
                {
                    pause.WaitOne();
                    if (interrupt)
                    {
                        State = BackupStateEnum.INTERRUPTED;
                        UpdateState();
                        pause.Release();
                        return;
                    }

                    Process process = IsBusinessAppRunning();
                    if(process != null)
                    {
                        State = BackupStateEnum.PAUSE;
                        UpdateState();
                        process.WaitForExit();
                    }

                    BackupFile file = null;

                    bool HugeFile = false;
                    bool canExecuteHugeFile = false;

                    if (files.Count > 0 && hugeFiles.Count > 0)
                    {
                        if (GetFilePriority(hugeFiles.First.source.FullName) <= GetFilePriority(files.First.source.FullName))
                        {
                            if (HugeFiles.WaitOne(50))
                            {
                                file = hugeFiles.Dequeue();
                                HugeFile = true;
                                canExecuteHugeFile = true;
                            }
                            else
                            {
                                file = files.Dequeue();
                            }
                        }
                    }
                    else if (files.Count > 0)
                    {
                        file = files.Dequeue();
                    }
                    else if (hugeFiles.Count > 0)
                    {
                        file = hugeFiles.Dequeue();
                    }

                    if (file == null)
                    {
                        break;
                    }

                    State = BackupStateEnum.ACTIVE;
                    sourceFilePath = file.source.FullName;
                    targetFilePath = file.target.FullName;

                    // Edit the state JSON file with the informations we have on the save progression
                    UpdateState();

                    if (!HugeFile && !canExecuteHugeFile)
                    {
                        if (file.source.Length >= Model.Instance.sizeLimit * Model.Instance.SizeUnitSize)
                        {
                            HugeFile = true;
                            if (HugeFiles.WaitOne(50))
                            {
                                canExecuteHugeFile = true;
                            }
                            else
                            {
                                canExecuteHugeFile = false;
                            }
                        }
                        else
                        {
                            HugeFile = false;
                            canExecuteHugeFile = true;
                        }
                    }

                    if (canExecuteHugeFile)
                    {
                        long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        int encryptTime = 0;

                        // Copy the file into the target repository
                        bool encrypt = false;
                        foreach (string extension in Model.Instance.encryptedExtensions)
                        {
                            if (file.target.Name.ToLower().EndsWith(extension.ToLower()))
                            {
                                ProcessStartInfo Cryptosoft = new ProcessStartInfo("Cryptosoft.exe");
                                Cryptosoft.Arguments = $"\"{Model.Instance.EncryptionKey}\" \"{file.source.FullName}\" \"{file.target.FullName}\"";
                                Cryptosoft.UseShellExecute = false;
                                Cryptosoft.CreateNoWindow = true;
                                Process cryptoProcess = Process.Start(Cryptosoft);
                                cryptoProcess.WaitForExit();
                                encryptTime = cryptoProcess.ExitCode;
                                encrypt = true;
                            }
                        }
                        if (!encrypt)
                        {
                            file.source.CopyTo(file.target.FullName, true);
                        }
                        Thread.Sleep(500);

                        // Edit the log JSON file
                        Log(file.source.FullName, file.target.FullName, file.source.Length, DateTimeOffset.Now.ToUnixTimeMilliseconds() - start, encryptTime);
                        nbFilesLeftToDo--;

                        if (HugeFile)
                        {
                            HugeFiles.Release();
                        }
                    }
                    else
                    {
                        hugeFiles.Enqueue(file, GetFilePriority(file.source.FullName));
                    }
                    pause.Release();
                }
                // When we have copy all the files, edit the state JSON file to "END"
                sourceFilePath = "";
                targetFilePath = "";
                totalFilesToCopy = 0;
                nbFilesLeftToDo = 0;
                totalFilesSize = 0;
                forceProgress = true;
                State = BackupStateEnum.END;
                UpdateState();
            }
            else
            {
                //Console.WriteLine("Notepad ouvert");
            }
        }
        private long GetFiles(List<BackupFile> files, DirectoryInfo source, DirectoryInfo target)
        {
            if (!System.IO.Directory.Exists(targetDirectory))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(targetDirectory);
                }
                catch (DirectoryNotFoundException e)
                {
                    return -1;
                }
            }

            // To copy all the files in one directory to another directory. 
            // Get the files in the source folder. (To recursively iterate through 
            // all subfolders under the current directory)
            if (System.IO.Directory.Exists(sourceDirectory))
            {
                string[] sourcefiles = System.IO.Directory.GetFiles(source.ToString());
                string[] targetfiles = System.IO.Directory.GetFiles(target.ToString());
                long totalFileSize = 0;

                // Copy the files and overwrite destination files if they already exist. 
                foreach (string s in sourcefiles)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = System.IO.Path.GetFileName(s);
                    FileInfo sourceFile = new FileInfo(Path.Combine(source.ToString(), fileName));
                    FileInfo targetFile = new FileInfo(Path.Combine(target.ToString(), fileName));

                    if (targetFile.Exists)
                    {
                        // Condition : if the file has been modified in the source folder and that we execute a DIFFERENTIAL backup
                        if (sourceFile.LastWriteTime > targetFile.LastWriteTime && backupType == BackupType.DIFFERENTIAL)
                        {
                            // Count the total files size
                            totalFileSize += sourceFile.Length;
                            // Add the current file to the list "files"
                            files.Add(new BackupFile(sourceFile, targetFile));
                        }
                        else if (backupType == BackupType.FULL)
                        {
                            // Count the total files size
                            totalFileSize += sourceFile.Length;
                            // Add the current file to the list "files"
                            files.Add(new BackupFile(sourceFile, targetFile));
                        }
                    }
                    else
                    {
                        // Count the total files size
                        totalFileSize += sourceFile.Length;
                        // Add the current file to the list "files"
                        files.Add(new BackupFile(sourceFile, targetFile));
                    }
                }
                // if there are folders in the current folders then we create them
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                    totalFileSize += GetFiles(files, diSourceSubDir, nextTargetSubDir);
                }
                if (backupType == BackupType.FULL)
                {
                    foreach (string t in targetfiles)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(t);
                        FileInfo sourceFile = new FileInfo(Path.Combine(source.ToString(), fileName));

                        if (!sourceFile.Exists)
                        {
                            // Delete the files in the target repository if they've been delete in the source repository and that we have executed a FULL backup work
                            File.Delete(t);
                        }
                    }
                }
                return totalFileSize;
            }
            else
            {
                return -1;
            }
        }

        private int GetFilePriority(string file)
        {
            return Model.Instance.priorityFiles.Contains(file.ToLower()) ? 1 : 2;
        }

        private Process IsBusinessAppRunning()
        {
            if (Model.Instance.businessApp == null || Model.Instance.businessApp.Length <= 0)
                return null;

            string fileName = Path.GetFileName(Model.Instance.businessApp);
            Process[] processName = Process.GetProcessesByName(fileName.Substring(0, fileName.LastIndexOf('.')));
            if (processName.Length > 0)
            {
                return processName[0];
            }
            return null;
        }

        // Method to add logs via observer
        protected void Log(string sourceFile, string targetFile, long fileSize, long transfertTime, int encryptTime)
        {
            BackupLog log = new BackupLog(name, sourceFile, targetFile, fileSize, transfertTime, encryptTime, DateTime.Now.ToString("G"));
            foreach (IObserver<BackupLog> observer in logObservers)
            {
                observer.OnNext(log);
            }
        }

        // Method to update states via observer
        protected void UpdateState()
        {
            BackupState state = new BackupState(Id, name, sourceFilePath, targetFilePath, State, totalFilesToCopy, totalFilesSize, nbFilesLeftToDo, totalFilesToCopy > 0 ? Convert.ToInt32((totalFilesToCopy - nbFilesLeftToDo) * 100.0 / totalFilesToCopy) : (forceProgress ? 100 : 0));
            foreach (IObserver<BackupState> observer in stateObservers)
            {
                observer.OnNext(state);
            }
        }

        // Save the configuration in the config JSON file via observer
        public void Save()
        {
            foreach (IObserver<string> observer in saveObservers)
            {
                observer.OnNext("");
            }
        }

        public IDisposable Subscribe(IObserver<BackupLog> observer)
        {
            if (!logObservers.Contains(observer))
            {
                logObservers.Add(observer);
            }
            return new Unsubscriber<BackupLog>(logObservers, observer);
        }

        public IDisposable Subscribe(IObserver<BackupState> observer)
        {
            if (!stateObservers.Contains(observer))
            {
                stateObservers.Add(observer);
                UpdateState();
            }
            return new Unsubscriber<BackupState>(stateObservers, observer);
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!saveObservers.Contains(observer))
            {
                saveObservers.Add(observer);
            }
            return new Unsubscriber<string>(saveObservers, observer);
        }
    }

    internal class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> mObservers;
        private IObserver<T> mObserver;

        internal Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this.mObservers = observers;
            this.mObserver = observer;
        }

        public void Dispose()
        {
            if (mObservers.Contains(mObserver))
                mObservers.Remove(mObserver);
        }
    }

    internal class BackupFile
    {
        public FileInfo source { get; private set; }
        public FileInfo target { get; private set; }
        public BackupFile(FileInfo source, FileInfo target)
        {
            this.source = source;
            this.target = target;
        }
    }
}
