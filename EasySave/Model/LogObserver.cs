using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    class LogObserver : IObserver<BackupLog>
    {
        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(BackupLog value)
        {
            string jsonString = JsonConvert.SerializeObject(value, Formatting.Indented);

            string filePath = GetFilePath();
            if(!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                jsonString = $",\n{jsonString}";
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.Write(jsonString);
                sw.Close();
            }
        }

        private string GetFilePath()
        {
            return $@"log_{DateTime.Now.ToString("dd-MM-yyyy")}.json";
        }
    }
}
