using Synchronization.HashCodes;
using System;
using UnityEngine;

namespace Synchronization.Identification.Implementations
{
    public class MeshIdProvider : IIdProvider
    {
        public Type Type => typeof(Mesh);

        public int GetHashCode(object obj)
        {
            var mesh = (Mesh)obj;
            unchecked
            {
                var verts = 0;
                foreach (var o in mesh.vertices)
                    verts += 23 * o.GetHashCode();
                var triangles = 0;
                foreach (var o in mesh.triangles)
                    triangles += 23 * o.GetHashCode();
                var h = 23 * verts;
                h += 23 * triangles;
                return h;
            }
        }
    }
}
