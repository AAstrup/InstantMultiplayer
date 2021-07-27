using System;

namespace Synchronization.HashCodes
{
    public interface IIdProvider
    {
        Type Type { get; }
        int GetHashCode(object obj);
    }
}
