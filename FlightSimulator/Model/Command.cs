using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FlightSimulator.Model
{
    class Command
    {
        void connectToServer()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                  Properties.Settings.Default.FlightCommandPort);
            TcpClient client = new TcpClient();

            // connect to server
            client.Connect(ep);
            Console.WriteLine("You are connected");
            NetworkStream stream = client.GetStream();
            //BinaryReader reader = new BinaryReader(stream);
            //BinaryWriter writer = new BinaryWriter(stream);
        }
    }
}
