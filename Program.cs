using System;
using EasySave.Model;

namespace EasySave
{
    class Program
    {
        public static Program instance;
        public Language language { get; private set; }

        static void Main(string[] args)
        {
            instance = new Program();
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
