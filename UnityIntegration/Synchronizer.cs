﻿using Communication.Synchronization;
using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Delta.Services;
using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.UnityIntegration.Events;
using InstantMultiplayer.UnityIntegration.Monitors.GameObjectMonitors;
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
        public int OwnerId => SynchronizerId & 31;
        public IEnumerable<ComponentMonitor> ComponentMonitors => _monitoredComponents.Values;
        public bool Initialized => SynchronizerId != 0;

        internal bool _foreign;
        internal SynchronizerOrigin _origin;
        private Dictionary<int, ComponentMonitor> _monitoredComponents = new Dictionary<int, ComponentMonitor>();
        private AMemberMonitorBase[] _gameObjectMemberMonitors;

        private void Start()
        {
            if (!Initialized)
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
                    if (comp == null) continue;

                    var compMonitor = MonitorFactory.CreateComponentMonitor(counter++, comp);
                    if (!_foreign && comp is IForeignComponent && comp is Behaviour behaviour)
                    {
                        behaviour.enabled = false;
                    }
                    _monitoredComponents.Add(compMonitor.Id, compMonitor);
                }
                SynchronizeStore.Instance.Register(this, _foreign);
                _gameObjectMemberMonitors = GameObjectMonitorProvider.Instance.MonitoredMembers(gameObject).ToArray();
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

        internal void SetComponentMembersUpdated(float timestamp)
        {
            foreach (var comp in _monitoredComponents)
                foreach (var member in comp.Value.Members)
                    member.SetUpdated(member.GetValue(), timestamp);
        }

        private void OnDestroy()
        {
            EventHandlerProvider.Instance.SynchronizerDestroyed(this);
            SynchronizeStore.Instance?.Unregister(this);
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
            var gameObjectMembers = deltaProvider.GetDeltaMembers(_gameObjectMemberMonitors, SyncClient.Instance.SyncTime).ToArray();
            deltaContainer = new DeltaContainer
            {
                SynchronizerId = SynchronizerId,
                ClientFilter = ClientFilter?.ClientFilter ?? SyncClientFilterConstants.All,
                Components = deltaComps,
                GameObjectMembers = gameObjectMembers
            };
            return true;
        }

        public void ConsumeDeltaContainer(DeltaConsumer deltaConsumer, DeltaContainer deltaContainer)
        {
            if (deltaContainer == null) return;

            if (deltaContainer.GameObjectMembers != null && deltaContainer.GameObjectMembers.Length > 0)
                deltaConsumer.ConsumeDelta(deltaContainer.GameObjectMembers, _gameObjectMemberMonitors);

            foreach (var compDelta in deltaContainer.Components)
                if (_monitoredComponents.TryGetValue(compDelta.Id, out var monitComp))
                    deltaConsumer.ConsumeDelta(compDelta, monitComp);
        }

        private IEnumerable<DeltaComponent> GetDeltaComponents(DeltaProvider deltaProvider)
        {
            if (deltaProvider == null) return null;
            var timeStamp = SyncClient.Instance.SyncTime;
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
