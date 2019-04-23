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

        public Command()
        {
            this.setDictionary();
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

        public void setDictionary()
        {
            this.nameToPath.Add("elevator", "/controls/flight/elevator");
            this.nameToPath.Add("aileron", "/controls/flight/aileron");
            this.nameToPath.Add("throttle", "/controls/engines/current-engine/throttle");
            this.nameToPath.Add("rudder", "/controls/flight/rudder");
        }

        public void startClient()
        {
            Thread thread = new Thread(() => connectToServer());
            thread.Start();
        }

        void connectToServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                  Properties.Settings.Default.FlightCommandPort);
            TcpClient client = new TcpClient();
            TcpListener server = new TcpListener(endPoint);

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

            /*
            List<string> splitCommand;

            // splits the command
            splitCommand = textUser.Split('\n').ToList();

            int i;

            for (i = 0; i < splitCommand.Count; i++)
            {
                int len = splitCommand[i].Length - 1;
                if (splitCommand[i][len] == '\r')
                {
                    splitCommand[i].Remove(len);
                }
                splitCommand[i] += "\r\n";
            }

            */
            foreach (string command in splitCommands)
            {
                string totalCommands = command + "\r\n";
                byte[] buffer = Encoding.ASCII.GetBytes(totalCommands);
                networkStream.Write(buffer, 0, buffer.Length);
            }

            //******************************************************************************* check if inside or out
            Thread.Sleep(2000);
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
    }
}
