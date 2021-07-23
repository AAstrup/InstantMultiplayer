using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration.Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    [AddComponentMenu(EditorConstants.ComponentMenuName + "/" + nameof(SyncClient))]
    public sealed class SyncClient: MonoBehaviour
    {
        private Client _client;
        private static Dictionary<Type, IMessageController> _controllers;
        public bool useAzureServer = false;
        private string azureIp = "instantmultiplayercontainer.northeurope.azurecontainer.io";
        private int azurePort = 61001;
        public string ip = "localhost";
        public int port = 61001;


        private void Awake()
        {
            var usedIp = useAzureServer ? azureIp : ip;
            var usedPort = useAzureServer ? azurePort : azurePort;
            _client = new Client(usedIp, usedPort);
        }

        private void Start()
        {
            _controllers.Add(SyncMessageController.Instance.GetMessageType(), SyncMessageController.Instance);
            _controllers.Add(TextMessageController.Instance.GetMessageType(), TextMessageController.Instance);
        }

        private void Update()
        {
            if (_client.connected)
            {
                foreach (KeyValuePair<Type, IMessageController> controller in _controllers)
                {
                    if (controller.Value.TryGetMessage(out var msg))
                        _client.SendMessage(msg);
                }

                _client.Poll();
                while (_client.IncomingMessageQueue.TryDequeue(out var message))
                {
                    _controllers[message.GetType()].HandleMessage(message);
                }
            }
        }
    }
}
