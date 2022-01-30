using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    class BackupState
    {
        public string Name { get; private set; }
        public string SourceFilePath { get; private set; }
        public string TargetFilePath { get; private set; }
        public string State { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public long TotalFileSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public int Progression { get; private set; }

        public BackupState(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression)
        {
            Name = name;
            SourceFilePath = sourceFilePath;
            TargetFilePath = targetFilePath;
            State = state;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFileSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            Progression = progression;
        }

    }
}
