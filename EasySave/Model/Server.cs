using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Model
{
    class Server : IObserver<BackupState>, INotifyPropertyChanged
    {
        private static readonly string server = "127.0.0.1";
        private static readonly int port = 9050;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public BackupType BackupType { get; set; }
        public BackupStateEnum State { get; set; }
        private int _progression;
        public int Progression 
        { 
            get 
            {
                return _progression;
            }
            set 
            {
                _progression = value;
                OnPropertyChanged(nameof(Progression));
            }
        }
        public bool ForceProgress { get; set; }

        // Properties needeed to fill up the progression attribute
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }

        /*
        static void Main(string[] args)
        {
            Socket socket_server = Connect(server, port);
            Console.WriteLine($"Connecté avec l'adresse {server} et {port}");

            Socket socket_client = AcceptConnection(socket_server);
            ListenNetwork(socket_client);
            // Disconnect(socket_server);
        }
        */
        public Socket Connect(string server, int port)
        {
            // create the socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(server), port);

            // bind the listening socket to the port
            try
            {
                socket.Bind(endpoint);
            }
            catch (Exception e)
            {
                Console.WriteLine("Winsock error: " + e.ToString());
            }

            // start listening
            int backlog = 10;
            socket.Listen(backlog);
            Console.WriteLine("Serveur disponible à l'écoute ...");

            return socket;
        }

        public Socket AcceptConnection(Socket socket)
        {
            // Accept a simple Socket connection
            Socket mySocket = socket.Accept();
            return mySocket;
        }

        public int ListenNetwork(Socket client)
        {
            byte[] msg = Encoding.UTF8.GetBytes("Bienvenu sur le serveur");
            try
            {
                while (true)
                {
                    string read = "Essai";
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
                return (e.ErrorCode);
            }

        }

        public void Disconnect(Socket socket)
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

        public void OnCompleted()
        {}

        public void OnError(Exception error)
        {}

        public void OnNext(BackupState value)
        {
            Progression = value.Progression;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
