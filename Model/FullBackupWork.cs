﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasySave.Model
{
    class FullBackupWork : BackupWork
    {
        public override void ExecuteBackup()
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);

            ExecuteBackup(source, target);
        }
    }
}
