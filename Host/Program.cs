using Autofac;
using Host.Controllers;
using InstantMultiplayer.Communication.Match;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static Dictionary<Type, KeyValuePair<string, Action<object, TcpClient>>> controllers;
        
        //Configurations
        private static bool debugging = true;

        static void Main(string[] args)
        {
            RegisterDependencies();
            RegisterControllers();

            //Blocks forever
            ListenForNewClients().Wait();
        }

        private static Dictionary<Type, KeyValuePair<string, Action<object, TcpClient>>> RegisterControllers()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IMessageController).IsAssignableFrom(p) && !p.IsAbstract);

            controllers = new Dictionary<Type, KeyValuePair<string, Action<object, TcpClient>>>();
            foreach (var type in types)
            {
                // If it fails to resolve the controller were not registered in the dependencies
                var controller = (IMessageController)container.Resolve(type);
                controllers.Add(controller.GetHandlerType(), new KeyValuePair<string, Action<object, TcpClient>>(controller.GetType().Name, controller.GetMessageHandler()));
            }

            return controllers;
        }

        private static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PlayerConnectionsRepository>().SingleInstance();
            builder.RegisterType<MatchLoginController>().SingleInstance();
            builder.RegisterType<TextMessageController>().SingleInstance();

            container = builder.Build();
        }


        public static async Task ListenForNewClients()
        {
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
                        var type = data.GetType();
                        if (!controllers.ContainsKey(type))
                        {
                            throw new Exception("Message of unknown type:" + type.ToString());
                        }
                        controllers[type].Value.Invoke(data, client);
                        if(debugging)
                            Console.WriteLine(controllers[type].Key.ToString() + " handled request of type " + type.ToString());
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
