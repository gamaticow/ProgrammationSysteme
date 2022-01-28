using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace EasySave.Model
{
    abstract class BackupWork : IObservable<BackupLog>
    {
        private List<IObserver<BackupLog>> logObservers = new List<IObserver<BackupLog>>();

        public String name;
        public String sourceDirectory;
        public String targetDirectory;
        public BackupType backupType { get; protected set; }
        public abstract void ExecuteBackup();

        protected void ExecuteBackup(DirectoryInfo source, DirectoryInfo target)
        {
            List<BackupFile> files = new List<BackupFile>();
            GetFiles(files, source, target);

            foreach (BackupFile file in files)
            {
                file.source.CopyTo(file.target.FullName, true);
            }
        }
        private void GetFiles(List<BackupFile> files, DirectoryInfo source, DirectoryInfo target)
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
                            //sourceFile.CopyTo(targetFile.FullName, true);
                            files.Add(new BackupFile(sourceFile, targetFile));
                        }
                        else if (backupType == BackupType.FULL)
                        {
                            //sourceFile.CopyTo(targetFile.FullName, true);
                            files.Add(new BackupFile(sourceFile, targetFile));
                            }
                    }
                    else
                    {
                        //System.IO.File.Copy(s, targetFile.Name, true);
                        files.Add(new BackupFile(sourceFile, targetFile));
                        }
                }
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                    ExecuteBackup(diSourceSubDir, nextTargetSubDir);
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
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }

        public void Log(string sourceFile, string targetFile, int fileSize, double transfertTime)
        {
            BackupLog log = new BackupLog(name, sourceFile, targetFile, fileSize, transfertTime, DateTime.Now.ToString("G"));
            foreach(IObserver<BackupLog> observer in logObservers)
            {
                observer.OnNext(log);
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
