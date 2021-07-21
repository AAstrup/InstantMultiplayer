using InstantMultiplayer.Communication.Match;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                
                //var host = "localhost";
                var host = "instantmultiplayercontainer.northeurope.azurecontainer.io";// Container instance
                Console.WriteLine("Hello Client World! Host is:" + host);


                TcpClient tcpClient = new TcpClient(host, 61001);
                Console.WriteLine("tcpClient started");
                var stream = tcpClient.GetStream();
                Console.WriteLine("tcpClient.GetStream");
                var writer = new BinaryWriter(stream);
                Console.WriteLine("BinaryWriter");
                var reader = new BinaryReader(stream);
                Console.WriteLine("BinaryReader");
                IFormatter formatter = new BinaryFormatter();
                Console.WriteLine("new BinaryFormatter");
                while (true)
                {
                    Console.WriteLine("Write line and press enter send that message to server");
                    var msg = Console.ReadLine();
                    var timer = new Stopwatch();
                    timer.Start();

                    var objectToSend = new TestMessage() { Message = msg };
                    byte[] bytes;
                    using (MemoryStream memory = new MemoryStream())
                    {
                        formatter.Serialize(memory, objectToSend);
                        bytes = memory.ToArray();
                    }

                    writer.Write(bytes);

                    while (true)
                    {
                        if (tcpClient.Available == 0) continue;
                        NetworkStream networkStream = tcpClient.GetStream();

                        if (networkStream.DataAvailable)
                        {
                            var data = formatter.Deserialize(networkStream);

                            var test = data as TestMessage;
                            if (test != null)
                            {
                                Console.WriteLine("Client service: {0}", test.Message);
                                break;
                            }
                        }
                    }

                    timer.Stop();
                    Console.WriteLine("Roundtrip ms: {0}", timer.ElapsedMilliseconds);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
