using Synchronization.Extensions;
using Synchronization.HashCodes;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Synchronization.Identification.Implementations
{
    public class MaterialIdProvider : IIdProvider
    {
        public Type Type => typeof(Material);

        public int GetHashCode(object obj)
        {
            var material = (Material)obj;
            unchecked
            {
                var vals = material.GetShaderPropertyObjects();
                var code = 0;
                foreach (var v in vals)
                {
                    if (v == null)
                        continue;
                    var type = v.GetType();
                    if(IdFactory.Instance.RegisteredType(type))
                        code += 23 * IdFactory.Instance.GetId(v, type);
                    else
                        code += 23 * v.GetHashCode();
                }
                return code;
            }
        }
    }
}
