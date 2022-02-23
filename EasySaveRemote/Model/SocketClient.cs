using RemoteCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySaveRemote.Model
{
    public delegate void Update(string type);

    class SocketClient
    {
        public Update Update { get; set; }
        private TcpClient client;

        public SocketClient()
        {
            client = new TcpClient("127.0.0.1", 9050);

            Thread listener = new Thread(() =>
            {
                while (client.Connected)
                {
                    object obj = Receive();
                    if (obj == null)
                        continue;
                    if(obj.GetType() == typeof(InitPacket))
                    {
                        InitPacket packet = (InitPacket)obj;
                        Model.Instance.AvailableLanguages = packet.AvailableLanguages;
                        foreach (RemoteBackupWork rbw in packet.RemoteBackupWorks)
                        {
                            if (rbw.Id == 0)
                                continue;
                            Model.Instance.BackupWorks[rbw.Id] = rbw;
                        }
                        Model.Instance.Language = new Language(packet.LanguagePacket.Language, packet.LanguagePacket.Translations);
                        Update.Invoke("Init");
                    }
                    else if (obj.GetType() == typeof(LanguagePacket))
                    {
                        LanguagePacket packet = (LanguagePacket)obj;
                        Model.Instance.Language = new Language(packet.Language, packet.Translations);
                        Update.Invoke("Language");
                    }
                    else if (obj.GetType() == typeof(AddBackupPacket))
                    {
                        AddBackupPacket packet = (AddBackupPacket)obj;
                        if (packet.BackupWork.Id == 0)
                            return;
                        Model.Instance.BackupWorks[packet.BackupWork.Id] = packet.BackupWork;
                        Update("AddBackupWork");
                    }
                    else if (obj.GetType() == typeof(DeleteBackupPacket))
                    {
                        DeleteBackupPacket packet = (DeleteBackupPacket)obj;
                        if (packet.Id == 0)
                            return;
                        Model.Instance.BackupWorks.Remove(packet.Id);
                        Update("DeleteBackupWork");
                    }
                    else if (obj.GetType() == typeof(UpdateBackupPacket))
                    {
                        UpdateBackupPacket packet = (UpdateBackupPacket)obj;
                        if (packet.BackupWork.Id == 0)
                            return;
                        Model.Instance.BackupWorks[packet.BackupWork.Id] = packet.BackupWork;
                        Update("Update");
                    }
                }
            });
            listener.Start();
        }

        public void Send(object obj)
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

        private object Receive()
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
            client.Close();
        }

    }
}
