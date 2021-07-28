using Autofac;
using Host.Controllers;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Communication.Match;
using InstantMultiplayer.Communication.Serialization;
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
            CreateControllers();

            //Blocks forever
            ListenForNewClients().Wait();
        }

        private static Dictionary<Type, KeyValuePair<string, Action<object, TcpClient>>> CreateControllers()
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
            builder.RegisterType<Events>().SingleInstance();
            builder.RegisterType<MatchLoginController>().SingleInstance();
            builder.RegisterType<TextMessageController>().SingleInstance();
            builder.RegisterType<SyncMessageController>().SingleInstance();
            builder.RegisterType<HistoryController>().SingleInstance();

            container = builder.Build();
        }


        public static async Task ListenForNewClients()
        {
            var connectionRepo = container.Resolve<PlayerConnectionsRepository>();
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
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERROR " + e.Message);
            }
        }
    }
}
