using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class SpriteRendererMonitorProvider : IComponentMonitorProvider
    {
        public IEnumerable<Type> ComponentTypes()
        {
            return new Type[] { typeof(SpriteRenderer) };
        }

        public IEnumerable<AMemberMonitorBase> MonitoredMembers(Component componentInstance)
        {
            var spriteRenderer = (SpriteRenderer)componentInstance;
            return new AMemberMonitorBase[]
            {
                new UnityObjectMemberProvider().GetMonitor(spriteRenderer, typeof(SpriteRenderer).GetProperty(nameof(SpriteRenderer.sprite))),
                //new UnityObjectMemberProvider().GetMonitor(spriteRenderer, typeof(SpriteRenderer).GetProperty(nameof(SpriteRenderer.color)))
            };
        }
    }
}
