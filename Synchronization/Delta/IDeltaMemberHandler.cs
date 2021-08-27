using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InstantMultiplayer.Synchronization.Delta
{
    public interface IDeltaMemberHandler
    {
        ComponentMonitor HandledComponentMonitor(IEnumerable<ComponentMonitor> componentMonitors);
        AMemberMonitorBase HandledMemberMonitor(ReadOnlyCollection<AMemberMonitorBase> memberMonitorBases);
        void HandleDeltaMember(DeltaMember deltaMember);
        bool ShouldHandle(DeltaMember deltaMember);
    }
}
