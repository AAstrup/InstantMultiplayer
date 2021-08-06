using Communication.Synchronization;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta.Services;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class SyncMessageController : BaseMessageController<SyncMessage>
    {
        public static SyncMessageController Instance 
        { 
            get 
            {
                if (_instance == null)
                    _instance = new SyncMessageController();
                return _instance;
            }
        }

        private static SyncMessageController _instance;
        private readonly DeltaConsumer _deltaConsumer;
        private readonly DeltaProvider _deltaProvider;

        public SyncMessageController()
        {
            _deltaConsumer = new DeltaConsumer();
            _deltaProvider = new DeltaProvider();
        }

        public override void HandleMessage(SyncMessage syncMessage)
        {
            foreach (var delta in syncMessage.Deltas)
            {
                if (SynchronizeStore.Instance.IsIdExhausted(delta.SynchronizerId))
                    continue;

                if (SynchronizeStore.Instance.TryGet(delta.SynchronizerId, out var synchronizer))
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
                    Debug.Log("Creating GB for foreign synchronizer id: " + synchronizer.SynchronizerId);
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
                    synchronizer._foreign = true;
                    synchronizer._origin = SynchronizerOrigin.Sync;
                    synchronizer.Initialize();
                    synchronizer.ConsumeDeltaContainer(_deltaConsumer, delta);
                }
            }
        }

        public override bool TryGetMessage(out IMessage message)
        {
            var deltas = new List<DeltaContainer>();
            foreach (var synchronizer in SynchronizeStore.Instance.Synchronizers)
                if (synchronizer.TryGetDeltaContainer(_deltaProvider, out var delta))
                    deltas.Add(delta);
            if (deltas.Count > 0)
            {
                message = new SyncMessage
                {
                    Deltas = deltas
                };
                return true;
            }
            message = null;
            return false;
        }
    }
}
