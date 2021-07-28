﻿using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using Synchronization.Repositories;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class MeshRendererMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            return new Type[] { typeof(MeshRenderer) };
        }

        public IEnumerable<MemberMonitor> MonitoredMembers(Component componentInstance)
        {
            var meshRenderer = (MeshRenderer)componentInstance;
            return new MemberMonitor[]
            {
                //new MemberMonitor(() => MaterialRepository.GetMaterialId(meshRenderer.sharedMaterial), (id) => meshRenderer.material = MaterialRepository.GetMaterialFromId((int)id))
                new UnityObjectMemberProvider().GetMonitor(meshRenderer, typeof(MeshRenderer).GetProperty(nameof(MeshRenderer.material)))
            };
        }
    }
}
