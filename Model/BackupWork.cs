using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    abstract class BackupWork
    {

        public abstract void ExecuteBackup();

        protected void Log(string value)
        {

        }

    }
}
