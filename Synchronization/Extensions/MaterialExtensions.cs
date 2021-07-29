using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace InstantMultiplayer.Synchronization.Extensions
{
    public static class MaterialExtensions
    {
        public static List<object> GetShaderPropertyObjects(this Material material)
        {
            var vals = new List<object>();
            var shader = material.shader;
            for (int i = 0; i < shader.GetPropertyCount(); i++)
            {
                var type = shader.GetPropertyType(i);
                var name = shader.GetPropertyName(i);
                switch (type)
                {
                    case ShaderPropertyType.Color:
                        vals.Add(material.GetColor(name));
                        break;
                    case ShaderPropertyType.Vector:
                        vals.Add(material.GetVector(name));
                        break;
                    case ShaderPropertyType.Range:
                    case ShaderPropertyType.Float:
                        vals.Add(material.GetFloat(name));
                        break;
                    case ShaderPropertyType.Texture:
                        vals.Add(material.GetTexture(name));
                        break;
                }
            }
            return vals;
        }
    }
}
