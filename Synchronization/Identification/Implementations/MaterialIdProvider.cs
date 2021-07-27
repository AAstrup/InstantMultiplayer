using Synchronization.HashCodes;
using System;
using UnityEngine;

namespace Synchronization.Identification.Implementations
{
    public class MaterialIdProvider : IIdProvider
    {
        public Type Type => typeof(Material);

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
            /*var material = (Material)obj;
            var shader = material.shader;
            for (int i = 0; i < shader.GetPropertyCount(); i++)
            {
                var type = shader.GetPropertyType(i);
                var name = shader.GetPropertyName(i);
                Debug.Log(shader.GetPropertyName(i));
                switch (type)
                {
                    case ShaderPropertyType.Color:
                        material.SetColor(name, Material.GetColor(name));
                        break;
                    case ShaderPropertyType.Vector:
                        material.SetVector(name, Material.GetVector(name));
                        break;
                    case ShaderPropertyType.Range:
                    case ShaderPropertyType.Float:
                        material.SetFloat(name, Material.GetFloat(name));
                        break;
                    case ShaderPropertyType.Texture:
                        material.SetTexture(name, Material.GetTexture(name));
                        break;
                }
            }*/
        }
    }
}
