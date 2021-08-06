using InstantMultiplayer.Synchronization.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace InstantMultiplayer.Synchronization.Identification
{
    public class GenericIdProvider
    {
        public static int GetHashCode(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if(obj is System.Collections.IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                enumerator.Reset();
                unchecked
                {
                    var h = 0;
                    while (enumerator.MoveNext())
                    {
                        var val = enumerator.Current;
                        h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                    }
                    return h;
                }
            }
            var type = obj.GetType();
            if (type.IsEnum || type.IsSystemPrimitive() || type.IsUnityPrimitive())
                return obj.GetHashCode();
            unchecked
            {
                var h = 0;
                foreach (var field in type.GetRuntimeFields().Where(f => !f.IsObsolete() && f.IsPublic))
                {
                    var val = field.GetValue(obj);
                    h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                }
                foreach (var property in type.GetRuntimeProperties().Where(p => !p.IsObsolete() && p.IsPublicGetSetProperty()))
                {
                    try
                    {
                        var val = property.GetValue(obj);
                        h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                    }
                    catch(Exception)
                    {
                        h += 23;
                    }
                }
                return h;
            }
        }
    }
}
