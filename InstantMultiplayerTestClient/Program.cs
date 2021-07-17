using System;
using System.Net.Sockets;
using System.Text;

namespace InstantMultiplayerTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client World!");
            var host = "localhost";
            //var host = "127.0.0.1";
            TcpClient tcpClient = new TcpClient(host, 61001);
            while (true) {
                Console.WriteLine("Press any key to send to server");
                Console.ReadKey();
                var textToSend = "Hi from client";
                NetworkStream nwStream = tcpClient.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending : " + textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
        }
    }
}
