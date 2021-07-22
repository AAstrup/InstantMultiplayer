using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
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
        public List<Component> Components;

        [NonSerialized]
        public int SynchronizerId;

        internal Dictionary<int, ComponentMonitor> _monitoredComponents = new Dictionary<int, ComponentMonitor>();

        private void Start()
        {
            try
            {
                _monitoredComponents = Components
                    .Select(c => MonitorFactory.CreateComponentMonitor(c))
                    .ToDictionary(m => m.Id, m => m);
                SyncClient.Instance.Register(this);
            } catch(Exception e)
            {
                Debug.LogError($"Synchronizer {name} failed to initialize due to: {e}");
            }
        }

        private void OnDestroy()
        {
            SyncClient.Instance?.Unregister(this);
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
