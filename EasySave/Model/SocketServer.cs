using RemoteCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySave.Model
{
    class SocketServer : IObserver<BackupState>
    {
        private static string[] remoteTranslations =
        {
            "selected_backup_work"
        };

        private TcpListener server;
        private List<string> languages;
        private Dictionary<int, BackupState> states = new Dictionary<int, BackupState>();

        private List<TcpClient> clients = new List<TcpClient>();

        public SocketServer()
        {
            languages = new List<string>();
            foreach(LanguageType languageType in Enum.GetValues(typeof(LanguageType)))
            {
                languages.Add(languageType.ToString());
            }

            server = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050));
            server.Start();

            new Thread(() =>
            {
                while (true)
                {
                    TcpClient client = null;
                    try
                    {
                        client = server.AcceptTcpClient();
                    }
                    catch (Exception e)
                    {
                        break;
                    }

                    clients.Add(client);
                    SendInit(client);


                    Thread listener = new Thread(() =>
                    {
                        while (client.Connected)
                        {
                            object obj = Receive(client);
                            if (obj == null)
                                continue;
                            if (obj.GetType() == typeof(LanguagePacket))
                            {
                                LanguagePacket packet = (LanguagePacket)obj;
                                SendAll(GetLanguagePacket(packet.Language));
                            }
                            else if (obj.GetType() == typeof(CommandPacket))
                            {
                                CommandPacket packet = (CommandPacket)obj;
                                BackupWork backupWork = null;
                                foreach (BackupWork bw in Model.Instance.backupWorks)
                                {
                                    if(bw.Id == packet.Id)
                                    {
                                        backupWork = bw;
                                        break;
                                    }
                                }
                                
                                if(backupWork== null)
                                {
                                    continue;
                                }

                                switch (packet.Command)
                                {
                                    case "Play":
                                        backupWork.ExecuteBackup();
                                        break;
                                    case "Pause":
                                        backupWork.Pause();
                                        break;
                                    case "Stop":
                                        backupWork.Interupt();
                                        break;
                                }
                            }
                        }
                    });
                    listener.Start();
                }
            }).Start();
        }

        private void SendInit(TcpClient client)
        {
            InitPacket packet = new InitPacket();
            List<RemoteBackupWork> backups = new List<RemoteBackupWork>();
            foreach(BackupWork bw in Model.Instance.backupWorks)
            {
                backups.Add(GetBackupWork(bw.Id));
            }
            packet.RemoteBackupWorks = backups;

            packet.LanguagePacket = GetLanguagePacket(Model.Instance.language.languageType.ToString());

            packet.AvailableLanguages = languages;

            Send(client, packet);
        }

        private LanguagePacket GetLanguagePacket(string language)
        {
            LanguageType languageType = LanguageType.ENGLISH;

            foreach(LanguageType type in Enum.GetValues(typeof(LanguageType)))
            {
                if(type.ToString() == language)
                {
                    languageType = type;
                    break;
                }
            }

            Language translator = new Language(languageType);
            Dictionary<string, string> translations = new Dictionary<string, string>();
            foreach (string key in remoteTranslations)
            {
                translations.Add(key, translator.Translate(key));
            }
            foreach (string key in languages)
            {
                translations.Add(key, translator.Translate(key));
            }
            return new LanguagePacket() { Language = translator.languageType.ToString(), Translations = translations };
        }

        public void AddBackupWork(BackupWork backupWork)
        {
            SendAll(new AddBackupPacket() { BackupWork = GetBackupWork(backupWork.Id) });
        }

        public void DeleteBackupWork(BackupWork backupWork)
        {
            SendAll(new DeleteBackupPacket() { Id = backupWork.Id });
        }

        public void RenameBackupWork(BackupWork backupWork)
        {
            SendAll(new UpdateBackupPacket() { BackupWork = GetBackupWork(backupWork.Id) });
        }

        private void SendAll(object obj)
        {
            foreach (TcpClient client in clients)
            {
                if (client.Connected)
                {
                    Send(client, obj);
                }
            }
        }

        private void Send(TcpClient client, object obj)
        {
            try
            {
                byte[] userDataBytes;
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf1 = new BinaryFormatter();
                bf1.Serialize(ms, obj);
                userDataBytes = ms.ToArray();

                byte[] userDataLen = BitConverter.GetBytes((Int32)userDataBytes.Length);
                client.GetStream().Write(userDataLen, 0, 4);
                client.GetStream().Write(userDataBytes, 0, userDataBytes.Length);
            }
            catch (Exception e)
            { }
        }

        private object Receive(TcpClient client)
        {
            try
            {
                byte[] readMsgLen = new byte[4];
                client.GetStream().Read(readMsgLen, 0, 4);

                int dataLen = BitConverter.ToInt32(readMsgLen);
                byte[] readMsgData = new byte[dataLen];
                client.GetStream().Read(readMsgData, 0, dataLen);
                MemoryStream ms = new MemoryStream(readMsgData);
                BinaryFormatter bf1 = new BinaryFormatter();
                return bf1.Deserialize(ms);
            }
            catch (Exception e)
            { }
            return null;
        }

        public void Close()
        {
            server.Stop();
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(BackupState value)
        {
            states[value.Id] = value;
            SendAll(new UpdateBackupPacket() { BackupWork = GetBackupWork(value.Id) });
        }

        private RemoteBackupWork GetBackupWork(int id)
        {
            RemoteBackupWork remoteBackupWork = new RemoteBackupWork();

            BackupWork backupWork = null;
            foreach (BackupWork bw in Model.Instance.backupWorks)
            {
                if (bw.Id == id)
                {
                    backupWork = bw;
                }
            }

            if (backupWork != null)
            {
                remoteBackupWork.Id = backupWork.Id;
                remoteBackupWork.Name = backupWork.name;
                remoteBackupWork.Play = backupWork.State != BackupStateEnum.ACTIVE;
                remoteBackupWork.Pause = backupWork.State == BackupStateEnum.ACTIVE;
                remoteBackupWork.Stop = backupWork.State == BackupStateEnum.ACTIVE || backupWork.State == BackupStateEnum.PAUSE;
            }

            if (states.ContainsKey(id))
            {
                BackupState state = states[id];
                remoteBackupWork.Progress = state.Progression;
                string color = "#198754";
                string image = null;
                if (state.State == "ACTIVE")
                {
                    image = "../Resources/green_play.png";
                }
                else if (state.State == "PAUSE")
                {
                    color = "#ffc107";
                    image = "../Resources/orange_pause.png";
                }
                else if (state.State == "INTERRUPTED")
                {
                    color = "#dc3545";
                    image = "../Resources/red_stop.png";
                }
                else if (state.State == "END")
                {
                    image = null;
                }
                remoteBackupWork.Color = color;
                remoteBackupWork.Image = image;
            }
            else
            {
                remoteBackupWork.Progress = 0;
            }

            return remoteBackupWork;
        }
    }
}
