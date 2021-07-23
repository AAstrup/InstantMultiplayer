using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    [AddComponentMenu(EditorConstants.ComponentMenuName + "/" + nameof(SyncClient))]
    public sealed class SyncClient: MonoBehaviour
    {
        public static SyncClient Instance;

        public IClient _client;
        private DeltaConsumer _deltaConsumer;
        private DeltaProvider _deltaProvider;
        private int idCounter;
        private bool _connected;
        private Dictionary<int, Synchronizer> _synchronizers;

        public void Connect()
        {
            _connected = true;
        }

        internal void Register(Synchronizer synchronizer) {
            synchronizer.SynchronizerId = idCounter++;
            _synchronizers.Add(synchronizer.SynchronizerId, synchronizer); 
        }
        internal void Unregister(Synchronizer synchronizer) { _synchronizers.Remove(synchronizer.SynchronizerId); }

        private void Awake()
        {
            Instance = this;
            _client = new Client();
            _synchronizers = new Dictionary<int, Synchronizer>();
            _deltaConsumer = new DeltaConsumer();
            _deltaProvider = new DeltaProvider();
        }

        private void Update()
        {
            if (_connected)
            {
                var deltas = new List<DeltaContainer>();
                foreach(var synchronizer in _synchronizers.Values)
                    if (synchronizer.TryGetDeltaContainer(_deltaProvider, out var delta))
                        deltas.Add(delta);
                if (deltas.Count > 0)
                    _client.SendMessage(new SyncMessage
                    {
                        Deltas = deltas
                    });

                while (_client.TryRecieveMessage(out var message))
                    if (message is SyncMessage syncMessage)
                        foreach (var delta in syncMessage.Deltas)
                            if (_synchronizers.TryGetValue(delta.SynchronizerId, out var synchronizer))
                            {
                                //Update
                                synchronizer.ConsumeDeltaContainer(_deltaConsumer, delta);
                            } 
                            else
                            {
                                //Create
                                var gb = new GameObject();
                                synchronizer = gb.AddComponent<Synchronizer>();
                                synchronizer.SynchronizerId = delta.SynchronizerId;
                                synchronizer.ClientFilter = ScriptableObject.CreateInstance<SyncClientFilter>();
                                synchronizer.ClientFilter.ClientFilter = delta.ClientFilter;
                                foreach (var deltaComp in delta.Components)
                                {
                                    var compType = ComponentMapper.GetTypeFromCID(deltaComp.TypeId);
                                    var comp = compType == typeof(Transform) ?
                                        gb.GetComponent<Transform>() :
                                        gb.AddComponent(compType);
                                    if (synchronizer.Components == null)
                                        synchronizer.Components = new List<Component>();
                                    synchronizer.Components.Add(comp);
                                }
                                synchronizer.Initialize();
                                synchronizer.ConsumeDeltaContainer(_deltaConsumer, delta);
                            }
            }
        }
    }
}
