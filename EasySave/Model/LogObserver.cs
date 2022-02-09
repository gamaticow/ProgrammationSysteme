using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

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
            string output = "";
            LogType type = Model.Instance.logType;

            string filePath = GetFilePath(type);
            if(!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                output += ",\n";
            }

            if(type == LogType.JSON)
            {
                output += JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented);
            }
            else if(type == LogType.XML)
            {
                var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                XmlSerializer serializer = new XmlSerializer(value.GetType());
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (var stream = new StringWriter())
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, value, emptyNamespaces);
                    output = $"\n{stream.ToString()}";
                }
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.Write(output);
            }

        }

        private string GetFilePath(LogType logType)
        {
            string extension = "";
            if(logType == LogType.JSON)
            {
                extension = "json";
            }
            else if(logType == LogType.XML)
            {
                extension = "xml";
            }
            return $@"log_{DateTime.Now.ToString("dd-MM-yyyy")}.{extension}";
        }
    }
}
