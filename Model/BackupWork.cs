using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;


namespace EasySave.Model
{
    abstract class BackupWork : IObservable<BackupLog>, IObservable<BackupState>
    {
        private List<IObserver<BackupLog>> logObservers = new List<IObserver<BackupLog>>();
        private List<IObserver<BackupState>> stateObservers = new List<IObserver<BackupState>>();

        public string name { get; set; }
        public string sourceDirectory { get; private set; }
        public string targetDirectory { get; private set; }
        public BackupType backupType { get; protected set; }

        public BackupWork(string name, string sourceDirectory, string targetDirectory)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
        }

        public abstract void ExecuteBackup();

        protected void ExecuteBackup(DirectoryInfo source, DirectoryInfo target)
        {
            List<BackupFile> files = new List<BackupFile>();
            long totalFileSize = GetFiles(files, source, target);
            int nbFilesLeftToDo = files.Count;

            foreach (BackupFile file in files)
            {
                UpdateState(file.source.FullName, file.target.FullName, "ACTIVE", files.Count, totalFileSize, nbFilesLeftToDo);
                long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                file.source.CopyTo(file.target.FullName, true);
                Log(file.source.FullName, file.target.FullName, file.source.Length, DateTimeOffset.Now.ToUnixTimeMilliseconds() - start);
                nbFilesLeftToDo--;
                Thread.Sleep(10000);
            }
            UpdateState("", "", "END", 0, 0, 0);
        }
        private long GetFiles(List<BackupFile> files, DirectoryInfo source, DirectoryInfo target)
        {
            if (!System.IO.Directory.Exists(targetDirectory))
            {
                System.IO.Directory.CreateDirectory(targetDirectory);
            }

            // To copy all the files in one directory to another directory. 
            // Get the files in the source folder. (To recursively iterate through 
            // all subfolders under the current directory, see 
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously 
            //       in this code example. 
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
                        if (sourceFile.LastWriteTime > targetFile.LastWriteTime && backupType == BackupType.DIFFERENTIAL)
                        {
                            // now you can safely overwrite it
                            totalFileSize += sourceFile.Length;
                            files.Add(new BackupFile(sourceFile, targetFile));
                        }
                        else if (backupType == BackupType.FULL)
                        {
                            totalFileSize += sourceFile.Length;
                            files.Add(new BackupFile(sourceFile, targetFile));
                        }
                    }
                    else
                    {
                        totalFileSize += sourceFile.Length;
                        files.Add(new BackupFile(sourceFile, targetFile));
                    }
                }
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
                            File.Delete(t);
                        }
                    }
                }
                return totalFileSize;
            }
            else
            {
                return -1;
                Console.WriteLine("Source path does not exist!");
            }         
        }

        protected void Log(string sourceFile, string targetFile, long fileSize, long transfertTime)
        {
            BackupLog log = new BackupLog(name, sourceFile, targetFile, fileSize, transfertTime, DateTime.Now.ToString("G"));
            foreach(IObserver<BackupLog> observer in logObservers)
            {
                observer.OnNext(log);
            }
        }

        protected void UpdateState(string sourceFilePath, string targetFilePath, string backupState, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo)
        {
            BackupState state = new BackupState(name, sourceFilePath, targetFilePath, backupState, totalFilesToCopy, totalFilesSize, nbFilesLeftToDo, totalFilesToCopy > 0 ? Convert.ToInt32((totalFilesToCopy - nbFilesLeftToDo) * 100.0 / totalFilesToCopy) : 0);
            foreach (IObserver<BackupState> observer in stateObservers)
            {
                observer.OnNext(state);
            }
        }

        public void Save()
        {

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
