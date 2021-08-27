using Autofac;
using Communication.Match;
using GuerrillaNtp;
using Host.Controllers;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Communication.Serialization;
using InstantMultiplayer.Initialization;
using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.UnityIntegration.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace InstantMultiplayer
{
    class Program
    {
        public static DateTime InitialSyncedTimestamp;

        private static TcpListener listener;
        private static int port = 61001;
        private static IContainer container;
        private static Dictionary<Type, KeyValuePair<string, Action<object, TcpClient>>> controllers;
        
        //Configurations
        private static bool debugging = true;

        static void Main(string[] args)
        {
            using (var ntp = new NtpClient(Dns.GetHostAddresses("pool.ntp.org")[0]))
                InitialSyncedTimestamp = DateTime.Now + ntp.GetCorrectionOffset();

            RegisterDependencies();
            CreateControllers();
            TypeInitializer.Instance.Initialize();

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
            builder.RegisterType<SyncInstantiationEventMessageController>().SingleInstance();
            builder.RegisterType<SyncInvocationEventMessageController>().SingleInstance();
            builder.RegisterType<SyncDestroyEventMessageController>().SingleInstance();
            builder.RegisterType<ClientConnectedMessage>().SingleInstance();
            builder.RegisterType<ClientDisconnectedMessage>().SingleInstance();
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
                    if (client.Connected)
                    {
                        _ = Task.Run(() => ListenToConnectedClient(client));
                        Console.WriteLine($"ListenToConnectedClient");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERROR " + e.Message);
            }
        }

        private static void ListenToConnectedClient(TcpClient client)
        {
            Console.WriteLine("ListenToConnectedClient: {0}", client.Client.RemoteEndPoint);
            var events = container.Resolve<Events>();

            try
            {
                while (true)
                {
                    if (client.Available == 0) continue;
                    NetworkStream networkStream = client.GetStream();

                    if (networkStream.DataAvailable)
                    {
                        var data = new BinarySerializer().Deserialize(networkStream);
                        var type = data.GetType();
                        if (!controllers.ContainsKey(type))
                        {
                            throw new Exception("Message of unknown type:" + type.ToString());
                        }
                        controllers[type].Value.Invoke(data, client);
                        events.messageClientIdRecieved.Invoke(null, new KeyValuePair<IMessage, TcpClient>((IMessage)data, client));
                        if (debugging)
                            Console.WriteLine(controllers[type].Key.ToString() + " handled request of type " + type.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
