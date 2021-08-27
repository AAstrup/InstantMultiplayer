using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.RichMemberMonitors;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Monitors.GameObjectMonitors
{
    public sealed class GameObjectMonitorProvider
    {
        public static GameObjectMonitorProvider Instance => _instance ?? (_instance = new GameObjectMonitorProvider());
        private static GameObjectMonitorProvider _instance;

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(GameObject gameObject)
        {
            return new AMemberMonitorBase[]
            {
                new MemberMonitor<string>(nameof(GameObject.tag), () => gameObject.tag, (v) => gameObject.tag = v),
                new MemberMonitor<string>(nameof(GameObject.layer), () => gameObject.tag, (v) => gameObject.tag = v),
                new RichMemberMonitor<int?, Transform>(nameof(gameObject.transform.parent), 
                    () => GetParentSyncId(gameObject),
                    () => gameObject.transform.parent,
                    (v) => SetParentFromSyncId(gameObject, v))
            };
        }

        private int? GetParentSyncId(GameObject gameObject)
        {
            if (SynchronizeStore.Instance.TryGetFromIID(gameObject.GetInstanceID(), out var sync))
                return sync.SynchronizerId;
            return null;
        }

        private void SetParentFromSyncId(GameObject gameObject, int? syncId)
        {
            if(syncId.HasValue && SynchronizeStore.Instance.TryGet(syncId.Value, out var synchronizer))
            {
                gameObject.transform.SetParent(synchronizer.transform, false);
                return;
            }
            gameObject.transform.SetParent(null);
        }
    }
}
