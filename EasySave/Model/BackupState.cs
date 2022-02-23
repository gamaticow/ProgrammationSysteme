using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    class BackupState
    {
        [JsonIgnore]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string SourceFilePath { get; private set; }
        public string TargetFilePath { get; private set; }
        public string State { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public long TotalFileSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public int Progression { get; private set; }

        public BackupState(int id, string name, string sourceFilePath, string targetFilePath, BackupStateEnum state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression)
        {
            Id = id;
            Name = name;
            SourceFilePath = sourceFilePath;
            TargetFilePath = targetFilePath;
            State = state.ToString();
            TotalFilesToCopy = totalFilesToCopy;
            TotalFileSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            Progression = progression;
        }

    }
}
