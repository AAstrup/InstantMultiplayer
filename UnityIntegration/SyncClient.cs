﻿using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration.Controllers;
using Synchronization.Objects;
using Synchronization.Objects.Resources;
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

        private float _sendInterval;
        private float _lastSendTimestamp;

        private void Awake()
        {
            var resourceOutline = Resources.Load<ResourceOutline>(ResourceOutline.ResourcePath);
            if (resourceOutline != null)
            {
                ResourceRepository.Instance.Commit(resourceOutline.Entries, resourceOutline.GetInstanceID());
            }

            try
            {
                var usedIp = useAzureServer ? azureIp : ip;
                var usedPort = useAzureServer ? azurePort : azurePort;
                _client = new Client(usedIp, usedPort);
                _client.OnIdentified += (e, v) =>
                {
                    Debug.Log("Recieved local id " + v.LocalId);
                    SynchronizeStore.Instance.DigestLocalId(v.LocalId);
                };
                _sendInterval = 1 / 30f;
            }
            catch(Exception e)
            {
                Debug.LogError("SyncClient failed to connect: " + e.ToString());
            }
        }

        private void Start()
        {
            _controllers = new Dictionary<Type, IMessageController>();
            _controllers.Add(SyncMessageController.Instance.GetMessageType(), SyncMessageController.Instance);
            _controllers.Add(TextMessageController.Instance.GetMessageType(), TextMessageController.Instance);
        }

        private void Update()
        {
            if (_client?.connected ?? false)
            {
                _client.Poll();
                if (_client.identified)
                {
                    if (Time.time - _lastSendTimestamp > _sendInterval)
                    {
                        foreach (KeyValuePair<Type, IMessageController> controller in _controllers)
                        {
                            if (controller.Value.TryGetMessage(out var msg))
                            {
                                Debug.Log("Sending msg of type " + controller.Key.ToString());
                                _client.SendMessage(msg);
                            }
                        }
                        _lastSendTimestamp = Time.time;
                    }
                    while (_client.incomingMessageQueue.TryDequeue(out var message))
                    {
                        if (_controllers.TryGetValue(message.GetType(), out var controller))
                            controller.HandleMessage(message);
                    }
                }
            }
        }

        public void SendMessage(IMessage message)
        {
            _client.SendMessage(message);
        }
    }
}
