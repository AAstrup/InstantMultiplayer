using System;

namespace InstantMultiplayer.Synchronization.Identification
{
    public interface IIdProvider
    {
        Type Type { get; }
        int GetHashCode(object obj);
    }
}
