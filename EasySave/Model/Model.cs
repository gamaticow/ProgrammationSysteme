using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Model
{
    class Model
    {

        public static Model Instance { get; private set; } = new Model();

        public Language language { get; private set; }
        public List<BackupWork> backupWorks { get; private set; }
        public LogObserver logObserver { get; private set; }
        public StateObserver stateObserver { get; private set; }
        public SaveBackupObserver saveObserver { get; private set; }

        private Model()
        {
            // This class is a singleton
            logObserver = new LogObserver();
            stateObserver = new StateObserver();
            saveObserver = new SaveBackupObserver();

            ReadDataFile();
        }

        // Method that read the configuration JSON file and import all the objects in it 
        public void ReadDataFile()
        {
            LanguageType languageType = LanguageType.ENGLISH;
            backupWorks = new List<BackupWork>();
            if (File.Exists("EasySave.json"))
            {
                EasySaveConfig save = EasySaveConfig.fromJson(File.ReadAllText("EasySave.json"));
                languageType = save.language;
                backupWorks = save.GetBackupWorks();
            }
            language = new Language(languageType);
        }

        // Wite the new backup in the configuration JSON file
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
            if (name == null || name.Length == 0 || sourceDirectory == null || sourceDirectory.Length == 0 || targetDirectory == null || targetDirectory.Length == 0 || type == null || type.Length == 0)
            {
                return 3;
            }

            if (BackupNameExists(name))
            {
                return 2;
            }

            if (language.Translate("backuptype_full").ToLower().Equals(type.ToLower()))
            {
                BackupWork backupWork = new FullBackupWork(name, sourceDirectory, targetDirectory);
                backupWork.Subscribe(logObserver);
                backupWork.Subscribe(stateObserver);
                backupWork.Subscribe(saveObserver);
                backupWorks.Add(backupWork);
            }
            else if (language.Translate("backuptype_differential").ToLower().Equals(type.ToLower()))
            {
                BackupWork backupWork = new DifferentialBackupWork(name, sourceDirectory, targetDirectory);
                backupWork.Subscribe(logObserver);
                backupWork.Subscribe(stateObserver);
                backupWork.Subscribe(saveObserver);
                backupWorks.Add(backupWork);
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

        public void DeleteBackup(BackupWork backupWork)
        {
            backupWorks.Remove(backupWork);
            WriteDataFile();
        }
    }

}
