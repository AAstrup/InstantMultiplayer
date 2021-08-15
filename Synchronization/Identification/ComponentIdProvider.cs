using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.Synchronization.Monitored;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification
{
    public class ComponentIdProvider
    {
        public static int GetHashCode(Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            var type = component.GetType();
            
            var h = 0;

            if (MonitorFactory.Instance.TryGetProviderMemberMonitors(component, out var members))
            {
                foreach(var member in members)
                {
                    var val = member.GetValue();
                    if(val == null)
                    {
                        unchecked
                        {
                            h += 23;
                        }
                        continue;
                    }
                    var id = IdFactory.Instance.GetId(val);
                    unchecked
                    {
                        h += val == null ? 23 : 23 * id;
                    }
                }
                return h;
            }

            foreach (var field in type.GetRuntimeFields().Where(f => !f.IsObsolete() && f.IsPublic))
            {
                var val = field.GetValue(component);
                if (val == null)
                {
                    unchecked
                    {
                        h += 23;
                    }
                    continue;
                }
                var id = IdFactory.Instance.GetId(val);
                unchecked
                {
                    h += val == null ? 23 : 23 * id;
                }
            }
            foreach (var property in type.GetRuntimeProperties().Where(p => !p.IsObsolete() && p.IsPublicGetSetProperty()))
            {
                object val;
                try
                {
                    val = property.GetValue(component);
                    if (val == null)
                    {
                        unchecked
                        {
                            h += 23;
                        }
                        continue;
                    }
                }
                catch (Exception)
                {
                    h += 23;
                    continue;
                }
                var id = IdFactory.Instance.GetId(val);
                unchecked
                {
                    h += val == null ? 23 : 23 * id;
                }
            }
            return h;
        }
    }
}
