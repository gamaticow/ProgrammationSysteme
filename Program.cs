using System;
using EasySave.Model;
using EasySave.Controller;
using System.IO;
using System.Collections.Generic;

namespace EasySave
{
    class Program
    {
        public static Program instance;
        public Language language { get; private set; }
        public List<BackupWork> backupWorks { get; private set; }

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
            LanguageType languageType = LanguageType.ENGLISH;
            backupWorks = new List<BackupWork>();
            if(File.Exists("EasySave.json"))
            {
                EasySaveConfig save = EasySaveConfig.fromJson(File.ReadAllText("EasySave.json"));
                languageType = save.language;
                backupWorks = save.GetBackupWorks();
            }
            language = new Language(languageType);
        }

        public void WriteDataFile()
        {
            BackupWork n = new DifferentialBackupWork("Travail de sauvegarde 1", @"C:\Users\coren\Desktop\CESI\Programmation système\Projet\source", @"C:\Users\coren\Desktop\CESI\Programmation système\Projet\target");
            BackupWork b = new FullBackupWork("Travail de sauvegarde 2", "", "");

            EasySaveConfig save = new EasySaveConfig();
            save.language = LanguageType.ENGLISH;
            save.AddBackup(n);
            save.AddBackup(b);

            File.WriteAllText("EasySave.json", save.ToJson());
        }

        public void OpenMainController()
        {
            new MainController().Main();
        }

        public void OpenBackupController(BackupWork backupWork)
        {
            new BackupController(backupWork).BackupWorkInformation();
        }
        public void SetLanguage(LanguageType languageType)
        {
            language = new Language(languageType);
        }
    }
}
