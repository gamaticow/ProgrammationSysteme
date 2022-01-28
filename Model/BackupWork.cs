using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    abstract class BackupWork : IObservable<BackupLog>
    {
        private List<IObserver<BackupLog>> logObservers = new List<IObserver<BackupLog>>();

        public String name;
        public String sourceDirectory;
        public String targetDirectory;
        public String backupType;
        public abstract void ExecuteBackup();

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
}
