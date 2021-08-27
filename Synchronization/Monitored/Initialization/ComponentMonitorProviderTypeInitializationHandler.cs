using InstantMultiplayer.Initialization;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers;
using System;

namespace InstantMultiplayer.Synchronization.Monitored.Initialization
{
    internal class ComponentMonitorProviderTypeInitializationHandler : ATypeInitializationHandler<IComponentMonitorProvider>
    {
        public override void HandleGenericType(Type type)
        {
            var instance = Activator.CreateInstance(type) as IComponentMonitorProvider;
            if (instance == null)
                return;
            MonitorFactory.Instance.InternalComponentRegisterProvider(instance);
        }
    }
}
