using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Monitored
{
    public static class MonitorDefaults
    {
        public static IEnumerable<IComponentMonitorProvider> ComponentMonitors()
        {
            return new IComponentMonitorProvider[]
            {
                new TransformMonitorProvider(),
                new MeshFilterMonitorProvider(),
                new MeshRendererMonitorProvider()
            };
        }

        public static IEnumerable<IMemberMonitorProvider> MemberMonitors()
        {
            return new IMemberMonitorProvider[]
            {
                new UnityObjectMemberProvider(),
                new IIDReferableUnityObjectMemberProvider()
            };
        }
    }
}
