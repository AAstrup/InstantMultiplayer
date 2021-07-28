using Synchronization.HashCodes;
using System;
using UnityEngine;

namespace Synchronization.Identification.Implementations
{
    public class TextureIdProvider : IIdProvider
    {
        public Type Type => typeof(Texture);

        public int GetHashCode(object obj)
        {
            var texture = (Texture)obj;
            unchecked
            {
                var h = 23 * texture.name.GetHashCode();
                h += 23 * texture.width.GetHashCode();
                h += 23 * texture.height.GetHashCode();
                return h;
            }
        }
    }
}
