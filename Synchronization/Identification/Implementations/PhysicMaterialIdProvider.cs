using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification.Implementations
{
    public class PhysicMaterialIdProvider : IIdProvider
    {
        public Type Type => typeof(PhysicMaterial);

        public int GetHashCode(object obj)
        {
            var material = (PhysicMaterial)obj;
            unchecked
            {
                var code = 0;
                code += 23 * IdFactory.Instance.GetId(material.bounciness);
                code += 23 * IdFactory.Instance.GetId(material.dynamicFriction);
                code += 23 * IdFactory.Instance.GetId(material.staticFriction);
                code += 23 * IdFactory.Instance.GetId(material.bounceCombine);
                code += 23 * IdFactory.Instance.GetId(material.frictionCombine);
                return code;
            }
        }
    }
}
