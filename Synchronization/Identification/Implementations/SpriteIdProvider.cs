using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification.Implementations
{
    // NOTE: Within editor sprites are considered Textures
    public class SpriteIdProvider : IIdProvider
    {
        public Type Type => typeof(Sprite);

        public int GetHashCode(object obj)
        {
            var sprite = (Sprite)obj;
            return IdFactory.Instance.GetId(sprite.texture, typeof(Texture));
        }
    }
}
