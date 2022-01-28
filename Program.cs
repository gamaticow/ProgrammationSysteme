using System;
using EasySave.Model;
using EasySave.Controller;
using System.Collections.Generic;

namespace EasySave
{
    class Program
    {
        public static Program instance;
        public Language language { get; private set; }
        public List<BackupWork> backupWorks;

        static void Main(string[] args)
        {
            instance = new Program();
            instance.ReadDataFile();
            instance.OpenMainController();
        }

        private Program()
        {
            // This class is a singleton
            backupWorks = new List<BackupWork>();
            BackupWork backup1 = new FullBackupWork();
            backup1.name = "Backup 1";
            backupWorks.Add(backup1);
            BackupWork backup2 = new FullBackupWork();
            backup2.name = "Backup 2";
            backupWorks.Add(backup2);
        }

        public void ReadDataFile()
        {
            language = new Language(LanguageType.FRENCH);
        }

        public void WriteDataFile()
        {

        }

        public void OpenMainController()
        {
            new MainController().Main();
        }

        public void OpenBackupController(BackupWork backupWork)
        {
            //new BackupController(backupWork).BackupWorkInformation();
        }

        public void SetLanguage(LanguageType languageType)
        {
            language = new Language(languageType);
        }
    }
}
