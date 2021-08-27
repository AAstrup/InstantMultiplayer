using System;

namespace InstantMultiplayer.Initialization
{
    public interface ITypeInitializationHandler
    {
        void HandleType(Type type);
    }
}
