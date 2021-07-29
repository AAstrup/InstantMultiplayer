using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification.Implementations
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
