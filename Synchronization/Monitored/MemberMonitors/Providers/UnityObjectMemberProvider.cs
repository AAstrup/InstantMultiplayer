using InstantMultiplayer.Synchronization.Extensions;
using Synchronization.HashCodes;
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
                () => GetIdFromObject(memberHolder, memberInfo),
                (id) => SetObjectFromId(memberHolder, memberInfo, (int)id)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return memberInfo.DeclaringType.IsAssignableFrom(typeof(UnityEngine.Object));
        }

        private int GetIdFromObject(object memberHolder, MemberInfo memberInfo)
        {
            var obj = (Object)memberInfo.GetValueFromMemberInfo(memberHolder);
            var id = IdFactory.Instance.GetId(obj);
            return id;
        }

        private void SetObjectFromId(object memberHolder, MemberInfo memberInfo, int id)
        {
            if(ResourceRepository.Instance.TryGetObject(id, memberInfo.DeclaringType, out var obj))
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
            else if (ReferenceRepository.Instance.TryGetObject(id, memberInfo.DeclaringType, out obj))
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
            else
                memberInfo.SetValueFromMemberInfo(memberHolder, null);

            if (memberInfo.GetValueFromMemberInfo(memberHolder) is UnityEngine.Object o && o != null)
                Debug.Log(o.name);
        }
    }
}
