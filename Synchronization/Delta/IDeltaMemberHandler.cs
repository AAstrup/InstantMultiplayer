using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Delta
{
    public interface IDeltaMemberHandler
    {
        bool ForeignOnly { get; }
        ComponentMonitor ComponentMonitorSelect(IEnumerable<ComponentMonitor> componentMonitors);
        AMemberMonitorBase MemberMonitorSelector(AMemberMonitorBase[] memberMonitorBases);
        void HandleDeltaMember(DeltaMember deltaMember);
    }
}
