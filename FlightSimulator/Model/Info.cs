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
        Thread thread;
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

        public void openServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                  Properties.Settings.Default.FlightInfoPort);
            TcpListener server = new TcpListener(endPoint);

            //closeServer(server, client);
            this.thread = new Thread(() => getData(server));
            thread.Start();
        }

        public void getData(TcpListener server)
        {
            // opens server
            server.Start();

            // waits for connection
            Console.WriteLine("Waiting for client connections...");

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("Connected!");

            // after connection- start listen to the flight.


            NetworkStream stream = client.GetStream();

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
                Lon = float.Parse(splitInput[0]);
                Lat = float.Parse(splitInput[1]);
           
            }
        }

        public void closeServer(TcpListener server, TcpClient client)
        {
            this.toStop = true;
            this.thread.Abort();
            client.Close();
            server.Stop();
        }
    }
}