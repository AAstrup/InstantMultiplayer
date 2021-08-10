using InstantMultiplayer.Synchronization.Identification.Implementations;
using System;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Events
{
    public class EventHandlerProvider
    {
        public static EventHandlerProvider Instance => _instance ?? (_instance = new EventHandlerProvider());
        private static EventHandlerProvider _instance;

        public EventHandler<InstantiationEvent> InstantiationEventHandler;
        public EventHandler<DestroyEvent> DestroyEventHandler;

        private GameObjectIdProvider _gameObjectIdProvider;

        private EventHandlerProvider()
        {
            _gameObjectIdProvider = new GameObjectIdProvider();
        }

        internal void PrefabInstantiated(GameObject prefab, Synchronizer instanceSynchronizer)
        {
            var prefabHashCode = _gameObjectIdProvider.GetHashCode(prefab);
            InstantiationEventHandler?.Invoke(this, new InstantiationEvent
            {
                PrefabId = prefabHashCode,
                SynchronizerId = instanceSynchronizer.SynchronizerId,
                ClientFilter = instanceSynchronizer.ClientFilter.ClientFilter
            });
        }

        internal void SynchronizerDestroyed(Synchronizer synchronizer)
        {
            if (SynchronizeStore.Instance.IsIdExhausted(synchronizer.SynchronizerId))
                return;
            DestroyEventHandler?.Invoke(this, new DestroyEvent
            {
                SynchronizerId = synchronizer.SynchronizerId,
                ClientFilter = synchronizer.ClientFilter.ClientFilter
            });
        }
    }
}
