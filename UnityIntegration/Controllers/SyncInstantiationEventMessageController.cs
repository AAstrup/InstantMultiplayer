using Communication.Synchronization.Events;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization.Objects;
using InstantMultiplayer.UnityIntegration.Events;
using InstantMultiplayer.UnityIntegration.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class SyncInstantiationEventMessageController : BaseMessageController<SyncInstantiationEventMessage>
    {
        public static SyncInstantiationEventMessageController Instance => _instance ?? (_instance = new SyncInstantiationEventMessageController());
        private static SyncInstantiationEventMessageController _instance;

        private Queue<InstantiationEvent> _prefabInstantiationQueue;

        public SyncInstantiationEventMessageController()
        {
            _prefabInstantiationQueue = new Queue<InstantiationEvent>();
            EventHandlerProvider.Instance.InstantiationEventHandler += (s, v) => _prefabInstantiationQueue.Enqueue(v);
        }

        public override void HandleMessage(SyncInstantiationEventMessage syncMessage)
        {
            if (SynchronizeStore.Instance.IsIdExhausted(syncMessage.SynchronizerId))
                return;

            if (SynchronizeStore.Instance.TryGet(syncMessage.SynchronizerId, out var existingSynchronizer))
            {
                if (existingSynchronizer != null && existingSynchronizer._origin != SynchronizerOrigin.Sync)
                {
                    //TODO: MERGE WITH EXISTING; FOR NOW IGNORE!
                    return;
                }
            }

            if (!MacroRepository.Instance.TryGetObject(syncMessage.PrefabId, typeof(GameObject), out var prefab))
                throw new System.Exception("Failed to identify prefab with id " + syncMessage.PrefabId);

            var gb = UnityEngine.Object.Instantiate<GameObject>(prefab as GameObject);
            if (gb == null)
                throw new System.Exception("Failed to instantiate prefab retrieved from id " + syncMessage.PrefabId);

            var synchronizer = gb.GetComponent<Synchronizer>();
            if(synchronizer == null)
            {
                UnityEngine.Object.Destroy(gb);
                throw new System.Exception("Failed to get " + nameof(Synchronizer) + " component from instance of prefab retrieved from id " + syncMessage.PrefabId);
            }
            synchronizer.SynchronizerId = syncMessage.SynchronizerId;
            synchronizer.ClientFilter = ScriptableObject.CreateInstance<SyncClientFilter>();
            synchronizer.ClientFilter.ClientFilter = syncMessage.ClientFilter;
            synchronizer._foreign = true;
            synchronizer._origin = SynchronizerOrigin.Instantiation;
            synchronizer.Initialize();
            synchronizer.SetComponentMembersUpdated(syncMessage.Timestamp);
        }

        public override bool TryGetMessage(out IMessage message)
        {
            if(_prefabInstantiationQueue.TryDequeue(out var eventMessage))
            {
                message = new SyncInstantiationEventMessage()
                {
                    SynchronizerId = eventMessage.SynchronizerId,
                    PrefabId = eventMessage.PrefabId,
                    ClientFilter = eventMessage.ClientFilter,
                    Timestamp = eventMessage.Timestamp

                };
                return true;
            }
            message = null;
            return false;
        }
    }
}
