using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Synchronization.Objects
{
    public class ReferenceRepository
    {
        public static ReferenceRepository Instance => _instance ?? (_instance = new ReferenceRepository());
        private static ReferenceRepository _instance;

        private Dictionary<string, Dictionary<string, UnityEngine.Object>> _map;

        public ReferenceRepository()
        {
            _map = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();
            Commit(GetDefaultObjects());
        }

        public void Commit(IEnumerable<UnityEngine.Object> objects)
        {
            foreach (var obj in objects)
            {
                if (_map.TryGetValue(obj.GetType().FullName, out var map))
                {
                    if (_map.ContainsKey(obj.name))
                        throw new Exception($"Duplicate objects of same type and name: {obj.name} of {obj.GetType().FullName}");
                    else
                        map.Add(obj.name, obj);
                }
                else
                {
                    var newMap = new Dictionary<string, UnityEngine.Object>();
                    newMap.Add(obj.name, obj);
                    _map.Add(obj.GetType().FullName, newMap);
                }
            }
        }

        public bool TryGetObject(string name, Type type, out UnityEngine.Object obj)
        {
            if (_map.TryGetValue(type.FullName, out var map))
                return map.TryGetValue(name, out obj);
            obj = null;
            return false;
        }

        private IEnumerable<UnityEngine.Object> GetDefaultObjects()
        {
            return PrimitiveMeshPaths.Select(p => Resources.GetBuiltinResource<Mesh>(p));
        }

        private static string[] PrimitiveMeshPaths = new string[]
        {
            "New-Sphere.fbx",
            "New-Capsule.fbx",
            "New-Cylinder.fbx",
            "Cube.fbx",
            "New-Plane.fbx",
            "Quad.fbx"
        };
    }
}
