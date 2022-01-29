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
        public long FileSize { get; private set; }
        public long FileTransferTime { get; private set; }
        public string time { get; private set; }

        public BackupLog(string name, string sourceFile, string targetFile, long fileSize, long transfertTime, string time)
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
