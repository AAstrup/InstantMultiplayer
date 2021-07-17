using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace InstantMultiplayer
{
    class Program
    {
        private static TcpListener listener;
        private static int port = 61001;

        static void Main(string[] args)
        {
            //Blocks until completed
            ListenForNewClients(args).Wait();
        }

        private static async Task HandleClient(TcpClient clt)
        {
            Console.WriteLine($"Client connected!");
            await Task.Run(() => ListenForClient(clt));
        }

        private static async Task ListenForClient(TcpClient clt)
        {
            Console.WriteLine($"Listening for new client");
            while (clt.Connected)
            {
                Console.WriteLine($"...Checking incoming data from client");
                if (clt.Available > 0)
                {
                    Console.WriteLine($"Data incoming from Client");

                    //using NetworkStream ns = clt.GetStream();
                    //using StreamReader sr = new StreamReader(ns);
                    using StreamReader sr = new StreamReader(clt.GetStream());
                    //string msg = await sr.ReadToEndAsync();
                    string msg = sr.ReadToEnd();

                    Console.WriteLine($"DATA:{msg}");
                }
                Thread.Sleep(1000);
            }
        }

        public static async Task ListenForNewClients(string[] args)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server listener started");

            while (true)
            {
                Console.WriteLine($"Awaiting clients");
                if (listener.Pending())
                    await HandleClient(await listener.AcceptTcpClientAsync());
                else
                    await Task.Delay(1000); //<--- timeout
            }
        }
    }
}
