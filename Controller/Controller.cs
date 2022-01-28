using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Model;

namespace EasySave.Controller
{
    abstract class Controller
    {
        protected BackupWork[] backupWorks { 
            get
            {
                return Program.instance.backupWorks.ToArray();
            }
        }

        public string Translate(string key)
        {
            return Program.instance.language.Translate(key);
        }

    }
}
