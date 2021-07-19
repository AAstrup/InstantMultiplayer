using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Assets
{
    public class IdTestUI: MonoBehaviour
    {
        public InputField InputField;
        public Text Text;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if (int.TryParse(InputField.text, out var iid))
                {
                    var obj = UnityObjectHelper.FindObjectFromInstanceID(iid);
                    if (obj == null)
                        Text.text = "";
                    else
                    {
                        Text.text = $"{obj.name} [{obj.GetType()}]";
                        if (obj is TextureImporter textureImporter)
                            Text.text += textureImporter.assetPath;
                    }
                }
            }
        }
    }
}
