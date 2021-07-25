using Synchronization.Extensions;
using Synchronization.Objects;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class ResourceTest: MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public Text Text;
        private void Start()
        {
            Text.text = "";
            var name = MeshFilter.mesh.NonInstanceName();
            if (ReferenceRepository.Instance.TryGetObject(name, typeof(Mesh), out var obj))
                Text.text += obj.GetInstanceID().ToString() + "\n";
            if (ResourceRepository.Instance.TryGetObject(MeshRenderer.material.NonInstanceName(), typeof(Material), out obj))
                Text.text += obj.GetInstanceID().ToString() + "\n";
        }
    }
}
