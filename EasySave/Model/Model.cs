using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EasySave.Model
{
    class Model
    {
        private static Model _instance;
        public static Model Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Model();
                    _instance.ReadDataFile();
                }
                return _instance;
            }
        }

        public Language language { get; private set; }
        public List<BackupWork> backupWorks { get; private set; }
        public LogObserver logObserver { get; private set; }
        public LogType logType { get; set; } = LogType.XML;
        public StateObserver stateObserver { get; private set; }
        public SaveBackupObserver saveObserver { get; private set; }
        public List<string> encryptedExtensions { get; set; } = new List<string>();
        public string businessApp { get; set; }
        public MediaPlayer mediaPlayer { get; set; }
        public string EncryptionKey { get; private set; }
        public int sizeLimit { get; set; }
        public SizeUnit SizeUnit { get; set; }
        public long SizeUnitSize
        {
            get
            {
                switch (SizeUnit)
                {
                    case SizeUnit.Ko:
                        return 1024;
                    case SizeUnit.Mo:
                        return 1048576;
                    case SizeUnit.Go:
                        return 1073741824;
                    case SizeUnit.To:
                        return 1099511627776;
                }
                return 1024;
            }
        }
        public List<string> priorityFiles { get; set; } = new List<string>();
        public object Music { get; set; }

        public Server serverStateObserver { get; set; }

        private Model()
        {
            // This class is a singleton
            logObserver = new LogObserver();
            stateObserver = new StateObserver();
            saveObserver = new SaveBackupObserver();
            serverStateObserver = new Server();
            
            Socket socket_server = serverStateObserver.Connect("127.0.0.1", 9050);
            //Socket socket_client = serverStateObserver.AcceptConnection(socket_server);
            //serverStateObserver.ListenNetwork(socket_client);
        }

        // Method that read the configuration JSON file and import all the objects in it 
        public void ReadDataFile()
        {
            backupWorks = new List<BackupWork>();
            encryptedExtensions = new List<string>();
            LanguageType languageType = LanguageType.ENGLISH;
            if (File.Exists("EasySave.json"))
            {
                EasySaveConfig save = EasySaveConfig.fromJson(File.ReadAllText("EasySave.json"));
                languageType = save.language;
                encryptedExtensions = save.encryptedExtensions;
                businessApp = save.businessApp;
                backupWorks = save.GetBackupWorks();
                logType = save.logType;
                Music = save.Music;
                EncryptionKey = save.EncryptionKey;
                priorityFiles = save.priorityFiles;
                sizeLimit = save.sizeLimit;
                SizeUnit = save.SizeUnit;
            }
            language = new Language(languageType);
            if(SizeUnit == null)
            {
                SizeUnit = SizeUnit.Ko;
            }

            // Generation of an encryption key for Cryptosoft
            if (EncryptionKey == null)
            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random rng = new Random();
                EncryptionKey = "";
                for(int i = 0; i < 20; i++)
                {
                    EncryptionKey += chars.ElementAt(rng.Next(0, chars.Length));
                }
                WriteDataFile();
            }
        }

        // Wite the new backup in the configuration JSON file
        public void WriteDataFile()
        {
            EasySaveConfig save = new EasySaveConfig();
            foreach (BackupWork backupWork in backupWorks)
            {
                save.AddBackup(backupWork);
            }
            save.encryptedExtensions = encryptedExtensions;
            save.businessApp = businessApp;
            save.language = language.languageType;
            save.logType = logType;
            save.Music = Music;
            save.EncryptionKey = EncryptionKey;
            save.priorityFiles = priorityFiles;
            save.sizeLimit = sizeLimit;
            save.SizeUnit = SizeUnit;
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
        public int CreateBackupWork(string name, string sourceDirectory, string targetDirectory, BackupType type)
        {
            if (name == null || name.Length == 0 || sourceDirectory == null || sourceDirectory.Length == 0 || targetDirectory == null || targetDirectory.Length == 0)
            {
                return 3;
            }

            if (BackupNameExists(name))
            {
                return 2;
            }

            if (type == BackupType.FULL)
            {
                BackupWork backupWork = new FullBackupWork(name, sourceDirectory, targetDirectory);
                backupWork.Subscribe(logObserver);
                backupWork.Subscribe(stateObserver);
                backupWork.Subscribe(saveObserver);
                backupWork.Subscribe(serverStateObserver);
                backupWorks.Add(backupWork);
            }
            else if (type == BackupType.DIFFERENTIAL)
            {
                BackupWork backupWork = new DifferentialBackupWork(name, sourceDirectory, targetDirectory);
                backupWork.Subscribe(logObserver);
                backupWork.Subscribe(stateObserver);
                backupWork.Subscribe(saveObserver);
                backupWork.Subscribe(serverStateObserver);
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
