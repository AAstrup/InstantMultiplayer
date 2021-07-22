using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public interface IComponentMonitorProvider
    {
        IEnumerable<Type> ComponentTypes();
        IEnumerable<MemberMonitor> MonitoredMembers(Component componentInstance);
    }
}
