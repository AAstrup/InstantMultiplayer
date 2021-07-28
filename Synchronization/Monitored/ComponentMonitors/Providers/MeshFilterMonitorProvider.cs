using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using Synchronization.Repositories;
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

        public IEnumerable<MemberMonitor> MonitoredMembers(Component componentInstance)
        {
            var meshFilter = (MeshFilter)componentInstance;
            return new MemberMonitor[]
            {
                new UnityObjectMemberProvider().GetMonitor(meshFilter, typeof(MeshFilter).GetProperty(nameof(MeshFilter.mesh)))
                //new MemberMonitor(() => MeshRepository.GetMeshHashCode(meshFilter.sharedMesh), (hashCode) => meshFilter.mesh = MeshRepository.GetMeshFromHashCode((int)hashCode))
            };
        }
    }
}
