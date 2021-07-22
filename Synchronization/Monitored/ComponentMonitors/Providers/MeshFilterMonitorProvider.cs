using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class MeshFilterMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            yield return typeof(MeshFilter);
        }

        public IEnumerable<MemberMonitor> MonitoredMembers(Component componentInstance)
        {
            var meshFilter = (MeshFilter)componentInstance;
            return new MemberMonitor[]
            {
                new GenericUnityObjectMemberProvider().GetMonitor(meshFilter, typeof(MeshFilter).GetProperty(nameof(MeshFilter.mesh)))
            };
        }
    }
}
