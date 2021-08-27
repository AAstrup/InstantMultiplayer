using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.ComponentMapping
{
    public class ComponentMapper
    {
        public static ComponentMapper Instance => _instance ?? (_instance = new ComponentMapper());
        private static ComponentMapper _instance;

        private readonly Dictionary<int, Type> _cidToType;
        private readonly Dictionary<Type, int> _typeToCID;

        public static Type GetTypeFromCID(int cid)
        {
            return Instance._cidToType[cid];
        }

        public static bool TryGetTypeFromCID(int cid, out Type type)
        {
            return Instance._cidToType.TryGetValue(cid, out type);
        }

        public static int GetCIDFromType(Type type)
        {
            return Instance._typeToCID[type];
        }

        public static bool TryGetCIDFromType(Type type, out int cid)
        {
            return Instance._typeToCID.TryGetValue(type, out cid);
        }

        public Type[] IncludedTypes()
        {
            return _typeToCID.Keys.ToArray();
        }

        internal void RegisterType(Type componentType)
        {
            var id = componentType.FullName.GetHashCode();
            _cidToType.Add(id, componentType);
            _typeToCID.Add(componentType, id);
        }

        private ComponentMapper()
        {
            _cidToType = new Dictionary<int, Type>();
            _typeToCID = new Dictionary<Type, int>();
            //var idCounter = 1;
            /*var comp = typeof(Component);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.CustomAttributes.All(att => !att.AttributeType.Equals(typeof(AssemblyIsEditorAssembly)))))
            {
                try
                {
                    foreach (var type in asm.GetTypes().Distinct().OrderBy(t => t.Name))
                        if (comp.IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            var id = type.FullName.GetHashCode();
                            _cidToType.Add(id, type);
                            _typeToCID.Add(type, id);
                        }
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }*/
        }
    }
}
