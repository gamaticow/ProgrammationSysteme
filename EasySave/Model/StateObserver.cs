using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace EasySave.Model
{
    class StateObserver : IObserver<BackupState>
    {
        private object _lock = new object();
        private Dictionary<string, BackupState> backupStates = new Dictionary<string, BackupState>();

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(BackupState value)
        {
            lock (_lock)
            {
                backupStates[value.Name] = value;

                BackupState[] values = new BackupState[backupStates.Count];
                backupStates.Values.CopyTo(values, 0);
                string jsonString = JsonConvert.SerializeObject(values, Formatting.Indented);

                File.WriteAllText("backup-state.json", jsonString);
            }
        }
    }
}
