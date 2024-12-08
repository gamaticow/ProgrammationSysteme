using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasySave.Model
{
    class FullBackupWork : BackupWork
    {

        public FullBackupWork(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
            backupType = BackupType.FULL;
        }

        public override bool ExecuteBackup()
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);

            return ExecuteBackup(source, target);
        }
    }
}
