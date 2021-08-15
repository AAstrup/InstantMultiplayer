using InstantMultiplayer.Synchronization.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification
{
    public class GenericIdProvider
    {
        public static GenericIdProvider Instance => _instance ?? (_instance = new GenericIdProvider());
        private static GenericIdProvider _instance;

        private readonly Type _componentType;

        private GenericIdProvider()
        {
            _componentType = typeof(Component);
        }

        public static int GetHashCode(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (obj is System.Collections.IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                enumerator.Reset();
                var h = 0;
                while (enumerator.MoveNext())
                {
                    var val = enumerator.Current;
                    if (TypeExcluded(val.GetType()))
                    {
                        unchecked
                        {
                            h += 23;
                        }
                        continue;
                    }
                    unchecked
                    {
                        h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                    }
                }
                return h;
            }
            var type = obj.GetType();
            if (type.IsEnum || type.IsSystemPrimitive() || type.IsUnityPrimitive())
                return obj.GetHashCode();
            unchecked
            {
                var h = 0;
                foreach (var field in IncludedFieldInfo(type.GetRuntimeFields()))
                {
                    var val = field.GetValue(obj);
                    h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                }
                foreach (var property in IncludedPropertyInfos(type.GetRuntimeProperties()))
                {
                    try
                    {
                        var val = property.GetValue(obj);
                        h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                    }
                    catch (Exception)
                    {
                        h += 23;
                    }
                }
                return h;
            }
        }

        private static bool TypeExcluded(Type type)
        {
            return Instance._componentType.IsAssignableFrom(type);
        }

        private static IEnumerable<FieldInfo> IncludedFieldInfo(IEnumerable<FieldInfo> fieldInfos)
        {
            return fieldInfos.Where(f => !f.IsObsolete() && f.IsPublic && !TypeExcluded(f.FieldType));
        }

        private static IEnumerable<PropertyInfo> IncludedPropertyInfos(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(p => !p.IsObsolete() && p.IsPublicGetSetProperty() && !TypeExcluded(p.PropertyType));
        }
    }
}
