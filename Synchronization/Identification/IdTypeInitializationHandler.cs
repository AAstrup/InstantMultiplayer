using InstantMultiplayer.Initialization;
using System;

namespace InstantMultiplayer.Synchronization.Identification
{
    internal class IdTypeInitializationHandler : ATypeInitializationHandler<IIdProvider>
    {
        public override void HandleGenericType(Type type)
        {
            var instance = Activator.CreateInstance(type) as IIdProvider;
            if (instance == null)
                return;
            IdFactory.Instance.Register(instance);
        }
    }
}
