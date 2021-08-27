using InstantMultiplayer.Initialization;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers;
using System;

namespace InstantMultiplayer.Synchronization.Monitored.Initialization
{
    internal class MemberMonitorProviderTypeInitializationHandler : ATypeInitializationHandler<IMemberMonitorProvider>
    {
        public override void HandleGenericType(Type type)
        {
            var instance = Activator.CreateInstance(type) as IMemberMonitorProvider;
            if (instance == null)
                return;
            MonitorFactory.Instance.InternalMemberRegisterProvider(instance);
        }
    }
}
