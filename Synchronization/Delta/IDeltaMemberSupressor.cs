using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InstantMultiplayer.Synchronization.Delta
{
    public interface IDeltaMemberSuppressor
    {
        ComponentMonitor SuppressedComponentMonitor(IEnumerable<ComponentMonitor> componentMonitors);
        AMemberMonitorBase SuppressedMemberMonitor(ReadOnlyCollection<AMemberMonitorBase> memberMonitorBases);
        bool ShouldSuppress(DeltaMember deltaMember);
    }
}
