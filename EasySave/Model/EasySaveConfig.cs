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
        public List<Dictionary<string, string>> backupWorks { get; private set; }
        public object Music { get; set; }

        public EasySaveConfig()
        {
            backupWorks = new List<Dictionary<string, string>>();
        }

        public void AddBackup(BackupWork backupWork)
        {
            Dictionary<string, string> save = new Dictionary<string, string>();
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
                    if("FULL".Equals(backupData.GetValueOrDefault("type")))
                    {
                        BackupWork backupWork = new FullBackupWork(backupData.GetValueOrDefault("name"), backupData.GetValueOrDefault("sourceDirectory"), backupData.GetValueOrDefault("targetDirectory"));
                        //TODO backupWork.Subscribe(Program.instance.logObserver);
                        //TODO backupWork.Subscribe(Program.instance.stateObserver);
                        //TODO backupWork.Subscribe(Program.instance.saveObserver);
                        output.Add(backupWork);
                        names.Add(backupData.GetValueOrDefault("name"));
                    }
                    else if("DIFFERENTIAL".Equals(backupData.GetValueOrDefault("type")))
                    {
                        BackupWork backupWork = new DifferentialBackupWork(backupData.GetValueOrDefault("name"), backupData.GetValueOrDefault("sourceDirectory"), backupData.GetValueOrDefault("targetDirectory"));
                        //TODO backupWork.Subscribe(Program.instance.logObserver);
                        //TODO backupWork.Subscribe(Program.instance.stateObserver);
                        //TODO backupWork.Subscribe(Program.instance.saveObserver);
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
