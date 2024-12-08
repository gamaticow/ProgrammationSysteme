using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace EasySave.Model
{
    abstract class BackupWork : IObservable<BackupLog>, IObservable<BackupState>, IObservable<string>
    {
        private List<IObserver<BackupLog>> logObservers = new List<IObserver<BackupLog>>();
        private List<IObserver<BackupState>> stateObservers = new List<IObserver<BackupState>>();
        private List<IObserver<string>> saveObservers = new List<IObserver<string>>();

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
            }
        }
        public string sourceDirectory { get; private set; }
        public string targetDirectory { get; private set; }
        public BackupType backupType { get; protected set; }

        public BackupWork(string name, string sourceDirectory, string targetDirectory)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
        }

        public abstract bool ExecuteBackup();

        protected bool ExecuteBackup(DirectoryInfo source, DirectoryInfo target)
        {
            List<BackupFile> files = new List<BackupFile>();
            // Execute GetFiles() and get the total file size that will be write in the state JSON file
            long totalFileSize = GetFiles(files, source, target);
            if(totalFileSize < 0)
            {
                return false;
            }

            // Get the current file size that will be write in the state JSON file
            int nbFilesLeftToDo = files.Count;

            // Loop on each file we need to save 
            foreach (BackupFile file in files)
            {
                // Edit the state JSON file with the informations we have on the save progression
                UpdateState(file.source.FullName, file.target.FullName, "ACTIVE", files.Count, totalFileSize, nbFilesLeftToDo);
                long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // Copy the file into the target repository
                file.source.CopyTo(file.target.FullName, true);

                // Edit the log JSON file
                Log(file.source.FullName, file.target.FullName, file.source.Length, DateTimeOffset.Now.ToUnixTimeMilliseconds() - start);
                nbFilesLeftToDo--;
            }
            // When we have copy all the files, edit the state JSON file to "END"
            UpdateState("", "", "END", 0, 0, 0);
            return true;
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

        // Method to add logs via observer
        protected void Log(string sourceFile, string targetFile, long fileSize, long transfertTime)
        {
            BackupLog log = new BackupLog(name, sourceFile, targetFile, fileSize, transfertTime, DateTime.Now.ToString("G"));
            foreach(IObserver<BackupLog> observer in logObservers)
            {
                observer.OnNext(log);
            }
        }

        // Method to update states via observer
        protected void UpdateState(string sourceFilePath, string targetFilePath, string backupState, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo)
        {
            BackupState state = new BackupState(name, sourceFilePath, targetFilePath, backupState, totalFilesToCopy, totalFilesSize, nbFilesLeftToDo, totalFilesToCopy > 0 ? Convert.ToInt32((totalFilesToCopy - nbFilesLeftToDo) * 100.0 / totalFilesToCopy) : 0);
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
                UpdateState("", "", "END", 0, 0, 0);
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
