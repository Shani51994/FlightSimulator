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
    public class Info
    {
        private bool toStop;
        private float lon;
        private float lat;
        Thread startThread;
        TcpListener server;
        TcpClient client;
        private static Info instance = null;

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

        public void startServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                      Properties.Settings.Default.FlightInfoPort);
            this.server = new TcpListener(endPoint);

            // opens server
            this.server.Start();

            // waits for connection
            Console.WriteLine("Waiting for client connections...");

            this.client = server.AcceptTcpClient();

            Console.WriteLine("Connected!");

            getData();
        }

       
        public void openServer()
        {

            this.startThread = new Thread(() => startServer());
            startThread.Start();

            // after connection- start listen to the flight.
            //closeServer(server, client);
            //this.thread = new Thread(() => getData());
            //thread.Start();
        }
        
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

                Console.WriteLine(input);
                // splits the input
                splitInput = input.Split(',');

  
                // gets the lon and lat from the input
                FlightBoardViewModel.Instance.Lon = float.Parse(splitInput[0]);
                FlightBoardViewModel.Instance.Lat = float.Parse(splitInput[1]);
           
            }
        }

        public void closeServer()
        {
            this.toStop = true;
            this.startThread.Abort();
            this.client.Close();
            this.server.Stop();
        }
    }
}