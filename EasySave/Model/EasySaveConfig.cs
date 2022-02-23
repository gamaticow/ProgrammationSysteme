using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EasySave.Model
{
    class EasySaveConfig
    {
        public LanguageType language { get; set; }
        public List<string> encryptedExtensions { get; set; }
        public string businessApp { get; set; }
        public int BackupId { get; set; }
        public List<Dictionary<string, string>> backupWorks { get; private set; }
        public LogType logType { get; set; }
        public object Music { get; set; }
        public string EncryptionKey { get; set; }
        public List<string> priorityFiles { get; set; }
        public int sizeLimit { get; set; }
        public SizeUnit SizeUnit { get; set; }

        public EasySaveConfig()
        {
            backupWorks = new List<Dictionary<string, string>>();
        }

        public void AddBackup(BackupWork backupWork)
        {
            Dictionary<string, string> save = new Dictionary<string, string>();
            save["id"] = backupWork.Id.ToString();
            save["name"] = backupWork.name;
            save["sourceDirectory"] = backupWork.sourceDirectory;
            save["targetDirectory"] = backupWork.targetDirectory;
            save["type"] = backupWork.backupType.ToString();

            backupWorks.Add(save);
        }

        public List<BackupWork> GetBackupWorks()
        {
            List<BackupWork> output = new List<BackupWork>();
            List<string> names = new List<string>();

            foreach (Dictionary<string, string> backupData in backupWorks)
            {
                if(!names.Contains(backupData.GetValueOrDefault("name")))
                {
                    BackupType? type = null;
                    if("FULL".Equals(backupData.GetValueOrDefault("type")))
                    {
                        type = BackupType.FULL;
                    }
                    else if("DIFFERENTIAL".Equals(backupData.GetValueOrDefault("type")))
                    {
                        type = BackupType.DIFFERENTIAL;
                    }

                    if(type != null)
                    {
                        BackupWork backupWork = new BackupWork(Int32.Parse(backupData.GetValueOrDefault("id")), backupData.GetValueOrDefault("name"), backupData.GetValueOrDefault("sourceDirectory"), backupData.GetValueOrDefault("targetDirectory"), type.Value);
                        backupWork.Subscribe(Model.Instance.logObserver);
                        backupWork.Subscribe(Model.Instance.stateObserver);
                        backupWork.Subscribe(Model.Instance.saveObserver);
                        output.Add(backupWork);
                        names.Add(backupData.GetValueOrDefault("name"));
                    }
                }
            }

            return output;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static EasySaveConfig fromJson(string json)
        {
            return JsonConvert.DeserializeObject<EasySaveConfig>(json);
        }

    }
}
