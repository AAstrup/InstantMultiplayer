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

        private DeltaProvider _deltaProvider;
        private MonitoredComponent[] _monitoredComponents;

        private void Start()
        {
            _monitoredComponents = Components
                .Select(c => MonitorFactory.CreateComponentMonitor(c))
                .ToArray();
            //Register!
        }

        private void OnDestroy()
        {
            //Deregister!
        }

        public bool TryGetDeltaContainer(out DeltaContainer deltaContainer)
        {
            var deltaComps = GetDeltaComponents().ToArray();
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

        private IEnumerable<DeltaComponent> GetDeltaComponents()
        {
            var timeStamp = 0; //Demo for now
            foreach(var monitorComp in _monitoredComponents)
                if(_deltaProvider.TryGetDeltaComponent(monitorComp, timeStamp, out var deltaComponent))
                    yield return deltaComponent;
        }
    }
}
