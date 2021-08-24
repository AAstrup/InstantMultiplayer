using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;

namespace InstantMultiplayer.Synchronization.Extensions
{
    public static class MemberMonitorExtensions
    {
        public static void SetUpdatedValue(this IMemberMonitorBase memberMonitor, object obj, float timeStamp)
        {
            memberMonitor.SetValue(obj);
            memberMonitor.SetUpdated(obj, timeStamp);
        }
    }
}
