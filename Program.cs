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
            EasySaveConfig save = new EasySaveConfig();
            save.language = language.languageType;
            foreach (BackupWork backupWork in backupWorks)
            {
                save.AddBackup(backupWork);
            }

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
            WriteDataFile();
        }

        /**
         * Create new backup work and save it
         * @return status
         * status = 0 => Created
         * status = 1 => Unknown type
         * status = 2 => Name already used
         * status = 3 => Field(s) are empty
         */
        public int CreateBackupWork(string name, string sourceDirectory, string targetDirectory, string type)
        {
            if(name == null || name.Length == 0 || sourceDirectory == null || sourceDirectory.Length == 0 || targetDirectory == null || targetDirectory.Length == 0 || type == null || type.Length == 0)
            {
                return 3;
            }

            if(BackupNameExists(name))
            {
                return 2;
            }

            if(language.Translate("backuptype_full").ToLower().Equals(type.ToLower()))
            {
                backupWorks.Add(new FullBackupWork(name, sourceDirectory, targetDirectory));
            }
            else if(language.Translate("backuptype_differential").ToLower().Equals(type.ToLower()))
            {
                backupWorks.Add(new DifferentialBackupWork(name, sourceDirectory, targetDirectory));
            }
            else
            {
                return 1;
            }
            WriteDataFile();
            return 0;
        }

        public bool BackupNameExists(string name)
        {
            foreach (BackupWork backupWork in backupWorks)
            {
                if (backupWork.name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
