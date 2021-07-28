using Synchronization.HashCodes;
using System;
using UnityEngine;

namespace Synchronization.Identification.Implementations
{
    public class Texture2DIdProvider : IIdProvider
    {
        public Type Type => typeof(Texture2D);

        public int GetHashCode(object obj)
        {
            return IdFactory.Instance.GetId(obj, typeof(Texture));
        }
    }
}
