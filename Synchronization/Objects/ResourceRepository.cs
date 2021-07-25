using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronization.Objects
{
    public class ResourceRepository
    {
        public static ResourceRepository Instance => _instance ?? (_instance = new ResourceRepository());
        private static ResourceRepository _instance;

        private Dictionary<string, Dictionary<string, string>> _map;
        private List<int> _committedIds;

        public ResourceRepository()
        {
            _map = new Dictionary<string, Dictionary<string, string>>();
            _committedIds = new List<int>();
        }

        public void Commit(IEnumerable<ResourceEntry> entries, int id)
        {
            if (_committedIds.Contains(id)) return;
            _committedIds.Add(id);
            Commit(entries);
        }

        public void Commit(IEnumerable<ResourceEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (_map.TryGetValue(entry.TypeName, out var map))
                {
                    if (_map.ContainsKey(entry.Name))
                        throw new Exception("Duplicate objects of same type and name: " + entry);
                    else
                        map.Add(entry.Name, entry.Path);
                }
                else
                {
                    var newMap = new Dictionary<string, string>();
                    newMap.Add(entry.Name, entry.Path);
                    _map.Add(entry.TypeName, newMap);
                }
            }
        }

        public bool TryGetObject(string name, Type type, out UnityEngine.Object obj)
        {
            obj = null;
            foreach (var key in _map.Keys)
                Debug.Log(key);
            if (_map.TryGetValue(type.FullName, out var map))
            {
                Debug.Log("Resource load with " + name + " of " + type.FullName);
                if (map.TryGetValue(name, out var path))
                {
                    obj = Resources.Load(path, type);
                    Debug.Log((obj == null));
                }
            }
            return obj != null;
        }
    }
}
