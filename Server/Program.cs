using SharedMessages;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstantMultiplayer
{
    class Program
    {
        private static TcpListener listener;
        private static int port = 61001;
        private static int listenPing = 100;

        static void Main(string[] args)
        {
            //Blocks forever
            ListenForNewClients(args).Wait();
        }

        public static async Task ListenForNewClients(string[] args)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server listener started");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                if (client.Connected)
                    await ListenToConnectedClient(client);
            }
        }

        private static async Task ListenToConnectedClient(TcpClient client)
        {
            try
            {
                while (true)
                {
                    if (client.Available == 0) continue;
                    NetworkStream networkStream = client.GetStream();

                    if (networkStream.DataAvailable)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        var data = formatter.Deserialize(networkStream);

                        var test = data as TestMessage;
                        if (test != null)
                        {
                            Console.WriteLine("[server] received TEST: {0}", test.Message);
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
    }
}
