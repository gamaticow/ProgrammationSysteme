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

            LogObserver logObserver = new LogObserver();
            BackupWork b = new FullBackupWork();
            b.name = "salut";
            b.Subscribe(logObserver);
            b.Log(@"C:\source\test.txt", @"D:\target\test.txt", 310, 3.810);
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
