using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.UnityIntegration.Interpolation;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Monitors.Components
{
    public class ASyncMemberInterpolatorBaseMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            return new Type[] { typeof(ASyncMemberInterpolatorBase) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var comp = (ASyncMemberInterpolatorBase)componentInstance;
            return new AMemberMonitorBase[]
            {
                new MemberMonitor<int>(nameof(ASyncMemberInterpolatorBase.Component), () => GetComponentId(comp), (cid) => SetComponentFromId(cid, comp)),
                new MemberMonitor<int>(nameof(ASyncMemberInterpolatorBase.SelectedIndex), () => comp.SelectedIndex, (i) => comp.SelectedIndex = i)
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
