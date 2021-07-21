using Autofac;
using InstantMultiplayer.Communication.Match;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace InstantMultiplayer
{
    class Program
    {
        private static TcpListener listener;
        private static int port = 61001;
        private static int listenPing = 100;
        private static IContainer container;

        static void Main(string[] args)
        {
            RegisterDependencies();

            //Blocks forever
            ListenForNewClients(args).Wait();
        }

        private static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PlayerConnections>().SingleInstance();

            container = builder.Build();
        }

        public static async Task ListenForNewClients(string[] args)
        {
            //IPAddress address;
            //IPAddress.TryParse("127.0.0.1", out address);
            ////IPAddress.TryParse("20.93.59.201", out address);
            //Console.WriteLine($"Attempt to create listener on IP " + address.ToString());
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                Console.WriteLine($"Server new TcpListener");
                listener.Start();
                Console.WriteLine($"Server listener started");

                while (true)
                {
                    Console.WriteLine($"Awaits new client");
                    var client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine($"listener.AcceptTcpClientAsync");
                    if (client.Connected)
                    {
                        Console.WriteLine($"client.Connected");
                        Task.Run(() => ListenToConnectedClient(client));
                        Console.WriteLine($"ListenToConnectedClient");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERROR " + e.Message);
            }
        }

        private static async Task ListenToConnectedClient(TcpClient client)
        {
            Console.WriteLine("ListenToConnectedClient: {0}", client.Client.RemoteEndPoint);
            var playerConnections = container.Resolve<PlayerConnections>();
            try
            {
                while (true)
                {
                    if (client.Available == 0) continue;
                    Console.WriteLine("Data recieved from: {0}", client.Client.RemoteEndPoint);
                    NetworkStream networkStream = client.GetStream();

                    if (networkStream.DataAvailable)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        var data = formatter.Deserialize(networkStream);

                        var loginMessage = data as MatchLoginRequest;
                        if (loginMessage != null)
                        {
                            playerConnections.AddPlayer(playerConnections.TEMPGetNextId(), client);
                        }
                        var testMsg = data as TestMessage;
                        if (testMsg != null)
                        {
                            foreach (var connection in playerConnections.TEMPGetAllPlayers())
                            {
                                SendToClient(connection.GetStream(), testMsg.Message);
                            }
                        }
                    }

                    Thread.Sleep(listenPing);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendToClient(NetworkStream networkStream, string message)
        {
            Console.WriteLine("[server] received TEST: {0}", message);
            BinaryFormatter formatter = new BinaryFormatter();
            var writer = new BinaryWriter(networkStream);
            var objectToSend = new TestMessage() { Message = "REPLY:" + message };
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
