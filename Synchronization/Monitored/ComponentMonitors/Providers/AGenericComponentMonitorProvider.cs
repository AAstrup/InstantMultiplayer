using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public abstract class AGenericComponentMonitorProvider<T>: IComponentMonitorProvider where T : Component
    {
        public IEnumerable<Type> ComponentTypes()
        {
            return new[] { typeof(T) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var monitors = new AMemberMonitorBase[MemberInfos.Length];
            for (var i = 0; i < MemberInfos.Length; i++)
                monitors[i] = MonitorFactory.CreateMemberMonitor(componentInstance, MemberInfos[i]);
            return monitors;
        }

        public abstract MemberInfo[] MemberInfos { get; }
    }
}
