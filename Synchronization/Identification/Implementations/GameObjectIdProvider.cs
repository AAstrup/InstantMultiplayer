using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification.Implementations
{
    public class GameObjectIdProvider: IIdProvider
    {
        public Type Type => typeof(GameObject);

        public int GetHashCode(object obj)
        {
            var gameObject = (GameObject)obj;
            unchecked
            {
                var h = 0;
                h += 23 * gameObject.layer;
                h += 23 * gameObject.tag.GetHashCode();
                foreach (var comp in gameObject.GetComponents(typeof(Component)))
                {
                    h += ComponentIdProvider.GetHashCode(comp);
                }
                return h;
            }
        }
    }
}
