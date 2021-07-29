using InstantMultiplayer.Synchronization.Identification;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class HashCodeTest: MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public Text Text;

        public void Start()
        {
            /*var vals = new List<System.Object>();
            var shader = MeshRenderer.material.shader;
            var material = MeshRenderer.material;
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

            unchecked
            {
                var code = 0;
                foreach (var v in vals)
                    code += 23 * (v == null ? 0 : v is Texture tex ? new TextureIdProvider().GetHashCode(tex) : v.GetHashCode());
                Debug.Log(code);
            }*/

            //var strs = string.Join(",", vals);
            //Renderer.material = material;
            var id = IdFactory.Instance.GetId(MeshRenderer.material);
            Text.text = id.ToString();
            Debug.Log(id);
        }
    }
}
