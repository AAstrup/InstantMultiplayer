using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.Synchronization.Monitored;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Identification
{
    public class ComponentIdProvider
    {
        public static int GetHashCode(Component component)
        {
            unchecked
            {
                var h = 0;
                h += 23 * component.GetType().FullName.GetHashCode();
                var memberInfos = MonitorFactory.Instance.MonitorableMemberInfos(component);
                foreach (var memberInfo in memberInfos)
                    h += IdFactory.Instance.TryGetId(memberInfo.GetValueFromMemberInfo(component), out int val) ? 23 * val : 0;
                return h;
            }
        }
    }
}
