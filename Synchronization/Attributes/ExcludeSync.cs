using System;

namespace InstantMultiplayer.Synchronization.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ExcludeSync: Attribute
    {
    }
}
