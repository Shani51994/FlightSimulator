using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{

    /**
     * this class responsible of the simulator connection to the client, and for transferring information from the simulator
     */
    public class Info
    {
        private bool toStop;
        private float lon;
        private float lat;
        Thread startThread;
        TcpListener server;
        TcpClient client;
        private static Info instance = null;



        /*
         * singleton to create only one instance of the info
        */
        public static Info Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Info();
                }

                return instance;
            }
        }

        // constructor to initialize 
        public Info()
        {
            lon = 0.0f;
            lat = 0.0f;
            toStop = false;
        }

        // Property of Lon
        public float Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
            }
        }

        // Property of Lat
        public float Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }

        /*
         * open and connect to the server, and send data from the simulator
         */
        public void startServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                      Properties.Settings.Default.FlightInfoPort);
            this.server = new TcpListener(endPoint);
          
            // opens server
            this.server.Start();
            this.client = server.AcceptTcpClient();
            getData();
        }

        /*
         * activate the startServer from a thread
         */
        public void openServer()
        {
            this.startThread = new Thread(() => startServer());
            startThread.Start();
        }

        /*
         * responsible to get the data from the simulator, need to gather each input until \n (end of data),
         * get data all the time that there is connection 
         */
        public void getData()
        {
           NetworkStream stream = this.client.GetStream();
           BinaryReader reader = new BinaryReader(stream);
           String[] splitInput;

            while (!toStop)
            {
                // read the input fron the simulator
                string input = "";
                char c;

                // read data untill \n
                while ((c = reader.ReadChar()) != '\n')
                {
                    input += c;
                }

                // splits the input
                splitInput = input.Split(',');

                // gets the lon and lat from the input and add them to the lon and lat in the instance
                FlightBoardViewModel.Instance.Lon = float.Parse(splitInput[0]);
                FlightBoardViewModel.Instance.Lat = float.Parse(splitInput[1]);
           
            }
        }

        /*
         * close the connection to the server, and the clients that are connected to him
         */
        public void closeServer()
        {
            this.toStop = true;
            this.startThread.Abort();
            this.client.Close();
            this.server.Stop();
        }
    }
}