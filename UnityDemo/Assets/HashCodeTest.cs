using Synchronization.HashCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class HashCodeTest: MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public Text Text;

        public void Start()
        {
            /*unsafe
            {
                var verts = 0;
                foreach (var o in MeshFilter.sharedMesh.vertices)
                    verts += 23 * o.GetHashCode();
                var triangles = 0;
                foreach (var o in MeshFilter.sharedMesh.triangles)
                    triangles += 23 * o.GetHashCode();
                var h = 23 * verts;
                h += 23 * triangles;
            }
            var val = MeshFilter.sharedMesh.vertices.Sum(x => x.x + x.y) + MeshFilter.sharedMesh.triangles.Sum(x => x);*/
            Text.text = IdFactory.Instance.GetId(MeshFilter.sharedMesh).ToString();
        }
    }
}
