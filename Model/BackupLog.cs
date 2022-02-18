using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    public class BackupLog
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public long FileTransferTime { get; set; }
        public string time { get; set; }

        public BackupLog()
        {

        }

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
