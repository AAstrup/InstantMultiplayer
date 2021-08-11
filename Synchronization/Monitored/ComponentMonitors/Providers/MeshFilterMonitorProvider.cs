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
            return new Type[] { typeof(MeshFilter) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var meshFilter = (MeshFilter)componentInstance;
            return new AMemberMonitorBase[]
            {
                new UnityObjectMemberProvider().GetMonitor(meshFilter, typeof(MeshFilter).GetProperty(nameof(MeshFilter.sharedMesh)))
                //new MemberMonitor(() => MeshRepository.GetMeshHashCode(meshFilter.sharedMesh), (hashCode) => meshFilter.mesh = MeshRepository.GetMeshFromHashCode((int)hashCode))
            };
        }
    }
}
