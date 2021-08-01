using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Objects
{
    public class ResourceRepository: ARepository<string>
    {
        public static ResourceRepository Instance => _instance ?? (_instance = new ResourceRepository());
        private static ResourceRepository _instance;

        private List<int> _committedIds;

        public ResourceRepository()
        {
            _map = new Dictionary<string, Dictionary<int, string>>();
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
                    if (map.ContainsKey(entry.Id))
                        throw new Exception("Duplicate objects of same type and name: " + entry);
                    else
                        map.Add(entry.Id, entry.Path);
                }
                else
                {
                    var newMap = new Dictionary<int, string>();
                    newMap.Add(entry.Id, entry.Path);
                    _map.Add(entry.TypeName, newMap);
                }
            }
        }

        public override bool TryGetObject(int id, Type type, out UnityEngine.Object obj)
        {
            obj = null;
            if (_map.TryGetValue(type.FullName, out var map))
            {
                Debug.Log("Resource load with id " + id + " of type " + type.FullName);
                if (map.TryGetValue(id, out var path))
                {
                    obj = Resources.Load(path, type);
                    Debug.Log((obj == null));
                }
            }
            return obj != null;
        }
    }
}
