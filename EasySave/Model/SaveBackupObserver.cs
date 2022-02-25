using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    class SaveBackupObserver : IObserver<string>
    {
        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(string value)
        {
            Model.Instance.WriteDataFile();
        }
    }
}
