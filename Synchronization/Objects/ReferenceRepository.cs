using Synchronization.HashCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Synchronization.Objects
{
    public class ReferenceRepository: ABaseRepository<UnityEngine.Object>
    {
        public static ReferenceRepository Instance => _instance ?? (_instance = new ReferenceRepository());
        private static ReferenceRepository _instance;

        internal ReferenceRepository()
        {
            _map = new Dictionary<string, Dictionary<int, UnityEngine.Object>>();
            Commit(GetDefaultObjects());
        }

        public void Commit(IEnumerable<UnityEngine.Object> objects)
        {
            foreach (var obj in objects)
            {
                if (_map.TryGetValue(obj.GetType().FullName, out var map))
                {
                    var id = IdFactory.Instance.GetId(obj);
                    if (map.ContainsKey(id))
                        throw new Exception($"Duplicate objects of same type and id: {id} of {obj.GetType().FullName}");
                    else
                    {
                        map.Add(id, obj);
                        //map.Names.Add(obj.name, obj);
                    }
                }
                else
                {
                    var newMap = new Dictionary<int, UnityEngine.Object>();
                    newMap.Add(IdFactory.Instance.GetId(obj), obj);
                    //newMap.Add(obj.name, obj);
                    _map.Add(obj.GetType().FullName, newMap);
                }
            }
        }

        public override bool TryGetObject(int id, Type type, out UnityEngine.Object obj)
        {
            if (_map.TryGetValue(type.FullName, out var map))
                return map.TryGetValue(id, out obj);
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
