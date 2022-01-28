using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    class BackupLog
    {
        public string Name { get; private set; }
        public string FileSource { get; private set; }
        public string FileTarget { get; private set; }
        public int FileSize { get; private set; }
        public double FileTransferTime { get; private set; }
        public string time { get; private set; }

        public BackupLog(string name, string sourceFile, string targetFile, int fileSize, double transfertTime, string time)
        {
            this.Name = name;
            this.FileSource = sourceFile;
            this.FileTarget = targetFile;
            this.FileSize = fileSize;
            this.FileTransferTime = transfertTime;
            this.time = time;
        }

    }
}
