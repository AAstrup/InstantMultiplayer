using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets
{
    public class ResourceTest: MonoBehaviour
    {
        public MeshRenderer Renderer;
        public Material Material;

        private void Start()
        {
            var shader = Shader.Find(Material.shader.name);
            var material = new Material(shader);
            for (int i=0; i<shader.GetPropertyCount(); i++)
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
            }

            Renderer.material = material;

            var oldiID = Material.GetInstanceID();
            var iID = material.GetInstanceID();
        }
    }
}
