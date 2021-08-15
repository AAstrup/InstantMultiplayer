using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification.Implementations
{
    public class GameObjectIdProvider: IIdProvider
    {
        public Type Type => typeof(GameObject);

        public int GetHashCode(object obj)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));

            var gameObject = (GameObject)obj;
            var h = 0;
            unchecked
            {
                h += 23 * gameObject.layer;
                h += 23 * gameObject.tag.GetHashCode();
            }
            var comps = gameObject.GetComponents(typeof(Component));
            foreach (var comp in comps)
            {
                if (comp is null) continue;
                try
                {
                    var id = ComponentIdProvider.GetHashCode(comp);
                    unchecked
                    {
                        h += 23 * id;
                    }
                }
                catch(Exception e)
                {
                    var msg = $"Failed to provide id for component {comp.GetType().FullName} for prefab {gameObject.name}";
                    Debug.LogError(msg + ": " + e.ToString());
                    throw new InvalidOperationException(msg, e);
                }
            }
            return h;
        }
    }
}
