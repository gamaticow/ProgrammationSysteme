using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemote.Model
{
    class Client
    {
        /*
      static void Main(string[] args)
      {
          Socket socket = Connect("127.0.0.1", 9050);
          ListenNetwork(socket);
          Disconnect(socket);
      }
      */

        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public BackupType BackupType { get; set; }
        public BackupStateEnum State { get; set; }
        public int Progression { get; set; }
        public bool ForceProgress { get; set; }

        // Properties needeed to fill up the progression attribute
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }


        private static Socket Connect(string server, int port)
        {
            // create the socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(server), port);

            // Connect to the remote endpoint
            socket.Connect(endpoint);

            return socket;
        }

        private static int ListenNetwork(Socket client)
        {
            try
            {

                while (true)
                {
                    string read = (string)Console.ReadLine();
                    byte[] bytes = new byte[256];


                    byte[] readBytes = Encoding.UTF8.GetBytes(read);
                    // Blocks until send returns.
                    int byteCount = client.Send(readBytes, 0, readBytes.Length, SocketFlags.None);

                    // Get reply from the server.
                    byteCount = client.Receive(bytes, 0, bytes.Length, SocketFlags.None);
                    string receivedString = Encoding.UTF8.GetString(bytes, 0, byteCount);

                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("{0} Error code: {1}.", e.Message, e.ErrorCode);
                return e.ErrorCode;
            }

        }

        private static void Disconnect(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                socket.Close();
            }

        }

        public void SetProgression()
        {
            this.Progression = this.TotalFilesToCopy > 0 ? Convert.ToInt32((this.TotalFilesToCopy - this.NbFilesLeftToDo) * 100.0 / this.TotalFilesToCopy) : (ForceProgress ? 100 : 0);
        }
    }
}
