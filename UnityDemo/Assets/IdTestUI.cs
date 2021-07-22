#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;  

namespace Assets
{
    public class IdTestUI: MonoBehaviour
    {
        public Object[] Objects;
        public Text ListText;
        public InputField InputField;
        public Text Text;
        public MeshRenderer MeshRenderer;

        private void Start()
        {
            var str = "";
            foreach (var obj in Objects)
                str += $"{obj.name} {obj.GetType()} {obj.GetInstanceID()}\n";
            ListText.text = str;


        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                var res = Resources.Load<Object>(InputField.text);
                if (res != null)
                    InputField.text = res.GetInstanceID().ToString();

                if (int.TryParse(InputField.text, out var iid))
                {
                    var obj = UnityObjectHelper.FindObjectFromInstanceID(iid);
                    if (obj == null)
                        Text.text = "";
                    else
                    {
                        Text.text = $"{obj.name} [{obj.GetType()}]";
#if UNITY_EDITOR
                        if (obj is TextureImporter textureImporter)
                            Text.text += textureImporter.assetPath;
#endif
                        if (obj is Material material)
                            MeshRenderer.material = material;
                        if (obj is Texture texture)
                            MeshRenderer.material.mainTexture = texture;
                    }   
                }
            }
        }
    }
}
