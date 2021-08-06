using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers.Collision
{
    public sealed class CircleMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            return new Type[] { typeof(UnityEngine.Transform) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var transform = (Transform)componentInstance;
            return new AMemberMonitorBase[]
            {
                new MemberMonitor<Vector3>(nameof(Transform.localPosition), () => transform.localPosition, (val) => transform.localPosition = (Vector3)val),
                new MemberMonitor<Quaternion>(nameof(Transform.localRotation), () => transform.localRotation, (val) => transform.localRotation = (Quaternion)val),
                new MemberMonitor<Vector3>(nameof(Transform.localScale), () => transform.localScale, (val) => transform.localScale = (Vector3)val)
            };
        }
    }
}
