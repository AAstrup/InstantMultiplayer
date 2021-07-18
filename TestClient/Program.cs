﻿using SharedMessages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("Hello Client World!");
                var host = "localhost";
                //var host = "20.93.59.201";// -- Container instance
                TcpClient tcpClient = new TcpClient(host, 61001);
                var stream = tcpClient.GetStream();
                var writer = new BinaryWriter(stream);
                var reader = new BinaryReader(stream);
                IFormatter formatter = new BinaryFormatter();
                while (true)
                {
                    Console.WriteLine("Write line and press enter send that message to server");
                    var msg = Console.ReadLine();

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
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
