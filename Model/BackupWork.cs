using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    abstract class BackupWork
    {
        public String name;
        public String sourceDirectory;
        public String targetDirectory;
        public String backupType;
        public abstract void ExecuteBackup();

        protected void Log(string value)
        {

        }

    }
}
