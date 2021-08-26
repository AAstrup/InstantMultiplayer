using Communication;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization.Objects;
using InstantMultiplayer.UnityIntegration.Controllers;
using Synchronization.Objects.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    [AddComponentMenu(EditorConstants.ComponentMenuName + "/" + nameof(SyncClient))]
    public sealed class SyncClient: MonoBehaviour
    {
        public static SyncClient Instance;

        public string ip = "localhost";
        public int port = 61001;
        public bool useAzureServer = false;
        public int SyncFrequency = 30;

        public float SyncTime => _client.SyncTime;
        public int LocalId => _client?.localId ?? 0;
        public bool Ready => _client != null && _client.connected && _client.identified;
        public event EventHandler<GreetMessage> OnIdentified;

        private Client _client;
        private static Dictionary<Type, IMessageController> _controllers;
        private Type[] _controllerMessageOrder;
        private string azureIp = "instantmultiplayercontainer.northeurope.azurecontainer.io";
        private int azurePort = 61001;

        private float _sendInterval => 1f / SyncFrequency;
        private float _lastSendTimestamp;

        void Awake()
        {
            Instance = this;
            Application.runInBackground = true;
        }

        private void Start()
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
                    SynchronizeStore.Instance.DigestLocalClientId(v.LocalId);
                    OnIdentified?.Invoke(this, v);
                };
            }
            catch(Exception e)
            {
                Debug.LogError("SyncClient failed to connect: " + e.ToString());
            }
            _controllers = new Dictionary<Type, IMessageController>();

            _controllers.Add(ClientConnectedMessageController.Instance.GetMessageType(), ClientConnectedMessageController.Instance);
            _controllers.Add(ClientDisconnectedMessageController.Instance.GetMessageType(), ClientDisconnectedMessageController.Instance);
            
            _controllers.Add(SyncInstantiationEventMessageController.Instance.GetMessageType(), SyncInstantiationEventMessageController.Instance);
            _controllers.Add(SyncDestroyEventMessageController.Instance.GetMessageType(), SyncDestroyEventMessageController.Instance);
            _controllers.Add(SyncMessageController.Instance.GetMessageType(), SyncMessageController.Instance);

            _controllers.Add(TextMessageController.Instance.GetMessageType(), TextMessageController.Instance);

            _controllerMessageOrder = new Type[]
            {
                ClientConnectedMessageController.Instance.GetMessageType(),
                ClientDisconnectedMessageController.Instance.GetMessageType(),
                SyncInstantiationEventMessageController.Instance.GetMessageType(),
                SyncDestroyEventMessageController.Instance.GetMessageType(),
                SyncMessageController.Instance.GetMessageType(),
                TextMessageController.Instance.GetMessageType()
            };
        }

        private void Update()
        {
            if (_client?.connected ?? false)
            {
                _client.Poll();
                if (_client.identified)
                {
                    while (_client.incomingMessageQueue.Count > 0)
                    {
                        var message = _client.incomingMessageQueue.Dequeue();
                        if (message == null)
                            continue;
                        try
                        {
                            if (_controllers.TryGetValue(message.GetType(), out var controller))
                                controller.HandleMessage(message);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(new Exception($"Failed to process {message.GetType()} message: ", e));
                        }
                    }
                    if (Time.time - _lastSendTimestamp > _sendInterval)
                    {
                        foreach (var messageType in _controllerMessageOrder)
                        {
                            try
                            {
                                var controller = _controllers[messageType];
                                if (controller.TryGetMessage(out var msg))
                                {
                                    Debug.Log("Sending msg of type " + messageType);
                                    _client.SendMessage(msg);
                                }
                            }
                            catch(Exception e)
                            {
                                Debug.LogException(new Exception($"Failed to get message of {messageType}: ", e));
                            }
                        }
                        _lastSendTimestamp = Time.time;
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
