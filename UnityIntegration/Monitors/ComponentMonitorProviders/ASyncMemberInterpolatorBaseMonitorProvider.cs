using InstantMultiplayer.Synchronization.ComponentMapping;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.UnityIntegration.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Monitors.Components
{
    public class ASyncMemberInterpolatorBaseMonitorProvider : IComponentMonitorProvider
    {
        private Type[] _cachedComponentTypes;

        public IEnumerable<Type> ComponentTypes()
        {
            if(_cachedComponentTypes == null)
            {
                var type = typeof(ASyncMemberInterpolatorBase);
                _cachedComponentTypes =
                    ComponentMapper.Instance.IncludedTypes()
                    .Where(t => type.IsAssignableFrom(t))
                    .ToArray();
            }
            return _cachedComponentTypes;
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var comp = (ASyncMemberInterpolatorBase)componentInstance;
            return new AMemberMonitorBase[]
            {
                new MemberMonitor<int>(nameof(ASyncMemberInterpolatorBase.Component), () => GetComponentId(comp), (cid) => SetComponentFromId(cid, comp)),
                new MemberMonitor<int>(nameof(ASyncMemberInterpolatorBase.SelectedIndex), () => comp.SelectedIndex, (i) => comp.SelectedIndex = i),
                new MemberMonitor<bool>(nameof(ASyncMemberInterpolatorBase.LocalLerping), () => comp.LocalLerping, (i) => comp.LocalLerping = i),
                new MemberMonitor<float>(nameof(ASyncMemberInterpolatorBase.LocalLerpScale), () => comp.LocalLerpScale, (i) => comp.LocalLerpScale = i)
            };
        }

        private int GetComponentId(ASyncMemberInterpolatorBase syncMemberInterpolatorBase)
        {
            return ComponentMapper.GetCIDFromType(syncMemberInterpolatorBase.Component.GetType());
        }

        private void SetComponentFromId(int id, ASyncMemberInterpolatorBase syncMemberInterpolatorBase)
        {
            var type = ComponentMapper.GetTypeFromCID(id);
            syncMemberInterpolatorBase.Component = syncMemberInterpolatorBase.gameObject.GetComponent(type);
        }
    }
}
