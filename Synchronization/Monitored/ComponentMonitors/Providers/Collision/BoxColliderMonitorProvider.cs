using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers.Collision
{
    public sealed class BoxColliderMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            return new Type[] { typeof(UnityEngine.BoxCollider) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var collider = (BoxCollider)componentInstance;
            return new AMemberMonitorBase[]
            {
                new MemberMonitor<bool>(nameof(BoxCollider.isTrigger), () => collider.isTrigger, (val) => collider.isTrigger = val),
                //IMPORTANT: Do not change to material from sharedmaterial; odd bug where retrieving material in editor will unassign the material from the BoxCollider...
                new UnityObjectMemberProvider().GetMonitor(collider, typeof(BoxCollider).GetProperty(nameof(BoxCollider.sharedMaterial))),
                new MemberMonitor<Vector3>(nameof(BoxCollider.center), () => collider.center, (val) => collider.center = val),
                new MemberMonitor<Vector3>(nameof(BoxCollider.size), () => collider.size, (val) => collider.size = val),
            };
        }
    }
}
