using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class TransformMonitorProvider : AGenericComponentMonitorProvider<Transform> //IComponentMonitorProvider
    {
        /*public IEnumerable<Type> ComponentTypes()
        {
            return new Type[] { typeof(Transform) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var transform = (Transform)componentInstance;
            return new AMemberMonitorBase[]
            {
                //new MemberMonitor<Vector3>(nameof(Transform.localPosition), () => transform.localPosition, (val) => transform.localPosition = (Vector3)val),
                new DiffMemberMonitorProvider().GetMonitor(transform, typeof(Transform).GetProperty(nameof(Transform.localPosition))),
                new MemberMonitor<Quaternion>(nameof(Transform.localRotation), () => transform.localRotation, (val) => transform.localRotation = (Quaternion)val),
                new MemberMonitor<Vector3>(nameof(Transform.localScale), () => transform.localScale, (val) => transform.localScale = (Vector3)val)
            };
        }*/
        public override MemberInfo[] MemberInfos { get; } = new MemberInfo[]
        {
            typeof(Transform).GetProperty(nameof(Transform.localPosition)),
            typeof(Transform).GetProperty(nameof(Transform.localRotation)),
            typeof(Transform).GetProperty(nameof(Transform.localScale))
        };
    }
}
