using System;
using EasySave.Model;
using System.IO;

namespace EasySave
{
    class Program
    {
        public static Program instance;
        public Language language { get; private set; }
        static void Main(string[] args)
        {
            instance = new Program();
            BackupWork n = new DifferentialBackupWork();
            n.sourceDirectory = @"C:\Users\rasor\OneDrive\Documents\test";
            n.targetDirectory = @"C:\Users\rasor\OneDrive\Documents\test2\test";

            n.ExecuteBackup();
        }

        private Program()
        {

        }

        public void ReadDataFile()
        {

        }

        public void WriteDataFile()
        {

        }

        public void OpenMainController()
        {

        }

        public void OpenBackupController(BackupWork backupWork)
        {

        }
        public void SetLanguage(LanguageType languageType)
        {
            language = new Language(languageType);
        }
    }
}
