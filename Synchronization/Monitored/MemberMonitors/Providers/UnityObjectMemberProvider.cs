using InstantMultiplayer.Synchronization.Extensions;
using Synchronization.Extensions;
using Synchronization.Objects;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class UnityObjectMemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue;

        public MemberMonitor GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new MemberMonitor(
                () => GetNameFromObject(memberHolder, memberInfo),
                (name) => SetObjectFromName(memberHolder, memberInfo, (string)name)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return memberInfo.DeclaringType.IsAssignableFrom(typeof(UnityEngine.Object));
        }

        private string GetNameFromObject(object memberHolder, MemberInfo memberInfo)
        {
            var obj = (Object)memberInfo.GetValueFromMemberInfo(memberHolder);
            return obj.NonInstanceName();
        }

        private void SetObjectFromName(object memberHolder, MemberInfo memberInfo, string name)
        {
            if(ResourceRepository.Instance.TryGetObject(name, memberInfo.DeclaringType, out var obj))
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
            else if (ReferenceRepository.Instance.TryGetObject(name, memberInfo.DeclaringType, out obj))
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
            else
                memberInfo.SetValueFromMemberInfo(memberHolder, null);
        }
    }
}
