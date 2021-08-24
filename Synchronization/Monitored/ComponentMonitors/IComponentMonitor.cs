using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors
{
    public interface IComponentMonitor : IMonitor
    {
        int Id { get; }
        Component MonitoredInstance { get; }
        ReadOnlyCollection<AMemberMonitorBase> Members { get; }
    }
}
