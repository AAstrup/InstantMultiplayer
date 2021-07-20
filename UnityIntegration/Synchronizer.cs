using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Monitored;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    [AddComponentMenu(EditorConstants.ComponentMenuName + "/" + nameof(Synchronizer))]
    public sealed class Synchronizer: MonoBehaviour
    {
        public SyncClientFilter ClientFilter;
        public List<MonoBehaviour> Components;

        [NonSerialized]
        public int SynchronizerId;

        internal Dictionary<int, MonitoredComponent> _monitoredComponents;

        private void Start()
        {
            _monitoredComponents = Components
                .ToDictionary(c => c.name.GetHashCode(), c => MonitorFactory.CreateComponentMonitor(c));
            SyncClient.Instance.Register(this);
        }

        private void OnDestroy()
        {
            SyncClient.Instance.Unregister(this);
        }

        public bool TryGetDeltaContainer(DeltaProvider deltaProvider, out DeltaContainer deltaContainer)
        {
            var deltaComps = GetDeltaComponents(deltaProvider).ToArray();
            if (deltaComps.Length == 0)
            {
                deltaContainer = null;
                return false;
            }
            deltaContainer = new DeltaContainer
            {
                SynchronizerId = SynchronizerId,
                Components = deltaComps
            };
            return true;
        }

        private IEnumerable<DeltaComponent> GetDeltaComponents(DeltaProvider deltaProvider)
        {
            var timeStamp = 0; //Demo for now
            foreach(var monitorComp in _monitoredComponents.Values)
                if(deltaProvider.TryGetDeltaComponent(monitorComp, timeStamp, out var deltaComponent))
                    yield return deltaComponent;
        }
    }
}
