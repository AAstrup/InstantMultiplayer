using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Synchronization.Repositories
{
    public class MaterialRepository : MonoBehaviour
    {
        // This should be automatically filled...
        public Material[] Materiales;
        private Dictionary<int, Material> hashCodeToId;
        private static MaterialRepository instance;

        public static MaterialRepository Instance { get => instance; }

        private void Awake()
        {
            instance = this;
            hashCodeToId = new Dictionary<int, Material>();
            foreach (var material in Materiales)
            {
                hashCodeToId.Add(GetMaterialId(material), material);
            }
        }

        public static int GetMaterialId(Material material)
        {
            var hashCode = material.name.GetHashCode();
            if (material.mainTexture != null)
                hashCode += material.mainTexture.width + material.mainTexture.height + material.mainTexture.name.GetHashCode();
            return hashCode;
        }

        public static Material GetMaterialFromId(int id)
        {
            return instance.hashCodeToId[id];
        }
    }

}
