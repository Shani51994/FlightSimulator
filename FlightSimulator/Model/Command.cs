using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FlightSimulator.Model
{
    class Command
    {
        private bool isConnected;
        public Dictionary<string, string> nameToPath = new Dictionary<string, string>();
        private static Command instance = null;
        private NetworkStream networkStream;
        private Thread thread;
        private TcpListener server;
        private TcpClient client;

        public Command()
        {
            this.isConnected = false;
        }

        public static Command Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Command();
                }

                return instance;
            }
        }

        public void startClient()
        {
            this.thread = new Thread(() => connectToServer());
            thread.Start();
        }

        void connectToServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                  Properties.Settings.Default.FlightCommandPort);
            this.client = new TcpClient();
            this.server = new TcpListener(endPoint);

            // connect to server
            client.Connect(endPoint);

            /*
            if (!client.Connected)
            {
                isConnected = false;
            }
            */

            Console.WriteLine("You are connected");

            isConnected = true;
            this.networkStream = client.GetStream();
            //BinaryReader reader = new BinaryReader(stream);
            //BinaryWriter writer = new BinaryWriter(stream);
        }

        public void sendToSimulator(string textUser)
        {
            if (!isConnected)
            {
                return;
            }

            string[] splitCommands = textUser.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string command in splitCommands)
            {
                string totalCommands = command + "\r\n";
                byte[] buffer = Encoding.ASCII.GetBytes(totalCommands);
                networkStream.Write(buffer, 0, buffer.Length);

                Thread.Sleep(2000);
            }

        }

        public void JoystickSendToSimulator(string textUser)
        {
            if (!isConnected)
            {
                return;
            }

            string totalCommands = textUser + "\r\n";
            byte[] buffer = Encoding.ASCII.GetBytes(totalCommands);
            networkStream.Write(buffer, 0, buffer.Length);
        }

        public void closeClient()
        {
            this.isConnected = false;
            this.thread.Abort();
            this.client.Close();
            this.server.Stop();
        }
    }
}
