using Communication.Synchronization.Events;
using InstantMultiplayer.Communication;
using InstantMultiplayer.UnityIntegration.Events;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class SyncDestroyEventMessageController : BaseMessageController<SyncDestroyEventMessage>
    {
        public static SyncDestroyEventMessageController Instance => _instance ?? (_instance = new SyncDestroyEventMessageController());
        private static SyncDestroyEventMessageController _instance;

        private Queue<DestroyEvent> _destroyQueue;

        public SyncDestroyEventMessageController()
        {
            _destroyQueue = new Queue<DestroyEvent>();
            EventHandlerProvider.Instance.DestroyEventHandler += (s, v) => _destroyQueue.Enqueue(v);
        }

        public override void HandleMessage(SyncDestroyEventMessage syncMessage)
        {
            if (SynchronizeStore.Instance.TryGet(syncMessage.SynchronizerId, out var synchronizer))
            {
                SynchronizeStore.Instance.ExhaustId(synchronizer.SynchronizerId);
                Object.Destroy(synchronizer.gameObject);
            }
        }

        public override bool TryGetMessage(out IMessage message)
        {
            if(_destroyQueue.TryDequeue(out var eventMessage))
            {
                message = new SyncDestroyEventMessage()
                {
                    SynchronizerId = eventMessage.SynchronizerId,
                    ClientFilter = eventMessage.ClientFilter
                };
                return true;
            }
            message = null;
            return false;
        }
    }
}
