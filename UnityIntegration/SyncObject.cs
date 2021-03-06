using InstantMultiplayer.UnityIntegration.Events;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    public static class SyncObject
    {
        public static GameObject Instantiate(GameObject prefab)
        {
            var gb = Object.Instantiate(prefab);
            if (!SyncClient.Instance.Ready)
            {
                Debug.LogWarning($"Using {nameof(SyncObject)}.{nameof(Instantiate)} while not connected is redundant.");
                return gb;
            }
            var synchronizer = gb.GetComponent<Synchronizer>();
            synchronizer.Initialize();
            var timestamp = SyncClient.Instance.SyncTime;
            synchronizer.SetComponentMembersUpdated(timestamp);
            EventHandlerProvider.Instance.PrefabInstantiated(prefab, synchronizer, timestamp);
            return gb;
        }
    }
}
