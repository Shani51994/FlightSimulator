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

    /**
     * this class responsible of the client connection to the simulator, and for transferring information from the client
     */
    class Command
    {
        private bool isConnected;
        private static Command instance = null;
        private NetworkStream networkStream;
        private Thread thread;
        private Thread sendThread;
        private TcpListener server;
        private TcpClient client;

        
        public Command()
        {
            this.isConnected = false;
        }

        /*
         * singleton to creat only one instance of the command
        */
        public static Command Instance
        {
            get
            {
                // if not exist create new else send the exist instance 
                if (instance == null)
                {
                    instance = new Command();
                }
                return instance;
            }
        }

        /*
         * activate the connectToServer from a thread
         */
        public void startClient()
        {
            this.thread = new Thread(() => connectToServer());
            thread.Start();
        }


        /*
         * open client and connect to the server
         */
        void connectToServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                  Properties.Settings.Default.FlightCommandPort);
            this.client = new TcpClient();
            this.server = new TcpListener(endPoint);

            // connect to server
            while (!client.Connected)
            {
                try { client.Connect(endPoint); }
                catch (Exception) { }
            }
          
            isConnected = true;
            this.networkStream = client.GetStream();
        }

        /*
         *sending commands from the auto pilot, after each command wait 2 sec for sending the next one
         */
        public void send(string textUser)
        {
            if (!isConnected)
            {
                return;
            }

            // split command by the current environment
            string[] splitCommands = textUser.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // for each command in the auto pilot, each command need to be send with \r\n
            foreach (string command in splitCommands)
            {
                string totalCommands = command + "\r\n";
                byte[] buffer = Encoding.ASCII.GetBytes(totalCommands);
                networkStream.Write(buffer, 0, buffer.Length);

                Thread.Sleep(2000);
            }

        }


        /*
         * activate the send of client from a thread (sending commands from the auto pilot)
         */
        public void sendToSimulator(string textUser)
        {
            this.sendThread = new Thread(() => send(textUser));
            this.sendThread.Start();
        }

        /*
         * send command after moving the joystick (add \r\n to the command)
         */
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



        /*
         * close the connection to the client, and the server that are connected to him
         */
        public void closeClient()
        {
            this.isConnected = false;
            if (this.sendThread != null) {
                this.sendThread.Abort();
            }
            this.thread.Abort();
            this.client.Close();
            this.server.Stop();
        }
    }
}
