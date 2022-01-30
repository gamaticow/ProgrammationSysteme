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

        public override void ExecuteBackup()
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);

            ExecuteBackup(source, target);
        }
    }
}
