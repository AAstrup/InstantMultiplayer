using InstantMultiplayer.Synchronization.Identification;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Objects
{
    public class ReferenceRepository: ARepository<UnityEngine.Object>
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
                    {
                        var conflictEntry = map[id];
                        throw new Exception($"Uniqueness conflict for type {obj.GetType().FullName} and id {id} between {conflictEntry.name} and {obj.name}. Please ensure that no identical objects exists.");
                    }
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
            var objects = new List<UnityEngine.Object>();
            foreach(var pair in DefaultResources)
            {
                foreach(var path in pair.Value)
                {
                    if (TryGetBuiltinResource(path, pair.Key, out var res))
                        objects.Add(res);
                }
            }
            return objects;
        }

        private bool TryGetBuiltinResource(string path, Type type, out UnityEngine.Object resource)
        {
            try
            {
                resource = Resources.GetBuiltinResource(type, path);
                return resource != null;
            }
            catch(Exception)
            {
                resource = null;
                return false;
            }
        }

        private Dictionary<Type, string[]> DefaultResources = new Dictionary<Type, string[]>
        {
            { typeof(Mesh), PrimitiveMeshPaths},
            { typeof(Material), DefaultMaterialPaths }
        };

        private static string[] PrimitiveMeshPaths = new string[]
        {
            "New-Sphere.fbx",
            "New-Capsule.fbx",
            "New-Cylinder.fbx",
            "Cube.fbx",
            "New-Plane.fbx",
            "Quad.fbx"
        };

        private static string[] DefaultMaterialPaths = new string[]
        {
            "Default-Diffuse.mat",
            "Default-Line.mat",
            "Default-Material.mat",
            "Default-Particle.mat",
            "Default-ParticleSystem.mat",
            "Default-Terrain-Diffuse.mat",
            "Default-Terrain-Specular.mat",
            "Default-Terrain-Standard.mat",
            "Default-Skybox.mat",
            "Sprites-Default.mat",
            "Sprites-Mask.mat",
            "SpatialMappingOcclusion.mat",
            "SpatialMappingWireframe.mat",
        };
    }
}
