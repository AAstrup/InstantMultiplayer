using UnityEngine;

namespace Assets
{
    public class IdTest: MonoBehaviour
    {
        public MeshRenderer MeshRenderer;

        private void Start()
        {
            var material = (Material)UnityObjectHelper.FindObjectFromInstanceID(13198);
            MeshRenderer.material = material;
        }
    }
}
