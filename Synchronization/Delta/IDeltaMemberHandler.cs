using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InstantMultiplayer.Synchronization.Delta
{
    public interface IDeltaMemberHandler
    {
        bool ForeignOnly { get; }
        ComponentMonitor ComponentMonitorSelect(IEnumerable<ComponentMonitor> componentMonitors);
        AMemberMonitorBase MemberMonitorSelector(ReadOnlyCollection<AMemberMonitorBase> memberMonitorBases);
        void HandleDeltaMember(DeltaMember deltaMember);
    }
}
