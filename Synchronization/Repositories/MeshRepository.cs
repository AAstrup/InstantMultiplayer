using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Synchronization.Repositories
{
    public class MeshRepository : MonoBehaviour
    {
        // This should be automatically filled...
        public Mesh[] meshes;
        private Dictionary<int, Mesh> hashCodeToMesh;
        private static MeshRepository instance;

        public static MeshRepository Instance { get => instance; }

        private void Awake()
        {
            instance = this;
            hashCodeToMesh = new Dictionary<int, Mesh>();
            foreach (var mesh in meshes)
            {
                hashCodeToMesh.Add(GetMeshHashCode(mesh), mesh);
            }
        }

        public static int GetMeshHashCode(Mesh mesh)
        {
            var hashCode = mesh.name.GetHashCode() + mesh.vertexCount;
            return hashCode;
        }

        public static Mesh GetMeshFromHashCode(int hashCode)
        {
            return instance.hashCodeToMesh[hashCode];
        }
    }

}
