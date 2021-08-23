using InstantMultiplayer.Synchronization.Extensions;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class Vector3MemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue;

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new DiffMemberMonitor<Vector3>(memberInfo.Name,
                () => (Vector3)memberInfo.GetValueFromMemberInfo(memberHolder),
                (v) => memberInfo.SetValueFromMemberInfo(memberHolder, v),
                (x, y) => x + y,
                (x, y) => x - y);
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return typeof(UnityEngine.Vector3).IsAssignableFrom(memberInfo.GetValueTypeFromMemberInfo());
        }
    }
}
