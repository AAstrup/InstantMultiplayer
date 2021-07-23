using InstantMultiplayer.Communication.Match;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace TestClient
{
    class Program
    {
        private static BinaryWriter writer;
        private static BinaryReader reader;

        static void Main(string[] args)
        {
            try
            {
                var host = "localhost";
                //var host = "instantmultiplayercontainer.northeurope.azurecontainer.io";// Container instance
                Console.WriteLine("Hello Client World! Host is:" + host);
                Thread.Sleep(1000);

                TcpClient tcpClient = new TcpClient(host, 61001);
                Console.WriteLine("tcpClient started");
                var stream = tcpClient.GetStream();
                Console.WriteLine("tcpClient.GetStream");
                writer = new BinaryWriter(stream);
                Console.WriteLine("BinaryWriter");
                reader = new BinaryReader(stream);
                Console.WriteLine("BinaryReader");
                IFormatter formatter = new BinaryFormatter();
                Console.WriteLine("new BinaryFormatter");
                SendMessage(new MessageMatchLogin() { });
                while (true)
                {
                    Console.WriteLine("Write line and press enter send that message to server");
                    Console.WriteLine("Or press enter to see new messages.");
                    var msg = Console.ReadLine();

                    if (msg.Length != 0)
                    {
                        SendMessage(new MessageText() { Message = msg });
                    }
                    
                    var timer = new Stopwatch();
                    timer.Start();
                    while (timer.ElapsedMilliseconds < 1000)
                    {
                        if (tcpClient.Available == 0) continue;
                        NetworkStream networkStream = tcpClient.GetStream();

                        if (networkStream.DataAvailable)
                        {
                            var data = formatter.Deserialize(networkStream);

                            var test = data as MessageText;
                            if (test != null)
                            {
                                Console.WriteLine("Client service: {0}", test.Message);
                                timer.Restart();
                            }
                        }
                    }
                    
                    timer.Stop();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void SendMessage(object objectToSend)
        {
            IFormatter formatter = new BinaryFormatter();
            byte[] bytes;
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, objectToSend);
                bytes = memory.ToArray();
            }

            writer.Write(bytes);
        }
    }
}
