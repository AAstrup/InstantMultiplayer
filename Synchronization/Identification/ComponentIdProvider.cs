﻿using InstantMultiplayer.Synchronization.Extensions;
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
            unchecked
            {
                var h = 0;

                if (MonitorFactory.Instance.TryGetProviderMemberMonitors(component, out var members))
                {
                    foreach(var member in members)
                    {
                        var val = member.GetValue();
                        h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                    }
                    return h;
                }

                foreach (var field in type.GetRuntimeFields().Where(f => !f.IsObsolete() && f.IsPublic))
                {
                    var val = field.GetValue(component);
                    h += val == null ? 23 : 23 * IdFactory.Instance.GetId(val);
                }
                foreach (var property in type.GetRuntimeProperties().Where(p => !p.IsObsolete() && p.IsPublicGetSetProperty()))
                {
                    try
                    {
                        var val = property.GetValue(component);
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
    }
}
