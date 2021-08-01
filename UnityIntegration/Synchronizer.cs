using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Delta.Services;
using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.UnityIntegration.Interpolation;
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

        public int SynchronizerId { get; internal set; }

        internal bool _foreign;
        private Dictionary<int, ComponentMonitor> _monitoredComponents = new Dictionary<int, ComponentMonitor>();

        private void Start()
        {
            if (_foreign)
                return;
            Initialize();
            LateInitialize();
        }

        internal void Initialize()
        {
            try
            {
                var counter = 0;
                foreach(var comp in Components ?? Enumerable.Empty<Component>())
                {
                    var compMonitor = MonitorFactory.CreateComponentMonitor(counter++, comp);
                    if (!_foreign && comp is IForeignComponent && comp is Behaviour behaviour)
                    {
                        behaviour.enabled = false;
                    }
                    _monitoredComponents.Add(compMonitor.Id, compMonitor);
                }
                SynchronizeStore.Instance.Register(this, _foreign);
            }
            catch (Exception e)
            {
                Debug.LogError($"Synchronizer {name} failed to initialize due to: {e}");
            }
        }

        internal void LateInitialize()
        {
            foreach (var compMonitor in _monitoredComponents.Values)
            {
                if (compMonitor.MonitoredInstance is IDeltaMemberHandler deltaMemberHandler)
                {
                    if (deltaMemberHandler.ForeignOnly && !_foreign)
                        continue;

                    var targetComp = deltaMemberHandler.ComponentMonitorSelect(_monitoredComponents.Values);
                    var member = targetComp == null ? null : deltaMemberHandler.MemberMonitorSelector(targetComp.Members);
                    if (member != null)
                    {
                        member.OnDeltaConsumed += (s, v) =>
                        {
                            deltaMemberHandler.HandleDeltaMember(v);
                        };
                    }
                }
            }
        }

        private void OnDestroy()
        {
            SynchronizeStore.Instance?.Unregister(this);
            //GameObjectDestroyMessageController.senddestroy TODO: Delete GameObject Controller
        }

        public bool TryGetDeltaContainer(DeltaProvider deltaProvider, out DeltaContainer deltaContainer)
        {
            if (deltaProvider == null)
            {
                deltaContainer = null;
                return false;
            }
            var deltaComps = GetDeltaComponents(deltaProvider)?.ToArray();
            if (deltaComps == null || deltaComps.Length == 0)
            {
                deltaContainer = null;
                return false;
            }
            deltaContainer = new DeltaContainer
            {
                SynchronizerId = SynchronizerId,
                ClientFilter = ClientFilter.ClientFilter,
                Components = deltaComps
            };
            return true;
        }

        public void ConsumeDeltaContainer(DeltaConsumer deltaConsumer, DeltaContainer deltaContainer)
        {
            if (deltaContainer == null) return;
            foreach (var compDelta in deltaContainer.Components)
                if (_monitoredComponents.TryGetValue(compDelta.Id, out var monitComp))
                    deltaConsumer.ConsumeDelta(compDelta, monitComp);
        }

        private IEnumerable<DeltaComponent> GetDeltaComponents(DeltaProvider deltaProvider)
        {
            if (_foreign) return null; //For now...
            if (deltaProvider == null) return null;
            var timeStamp = 0; //Demo for now
            var deltaComps = new List<DeltaComponent>();
            foreach (var monitorComp in _monitoredComponents.Values)
            {
                if (deltaProvider.TryGetDeltaComponent(monitorComp, timeStamp, out var deltaComponent))
                    deltaComps.Add(deltaComponent);
            }
            return deltaComps;
        }
    }
}
