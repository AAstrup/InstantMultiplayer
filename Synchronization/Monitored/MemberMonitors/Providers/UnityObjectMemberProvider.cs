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
                memberInfo.Name,
                () => GetIdFromObject(memberHolder, memberInfo),
                (id) => SetObjectFromId(memberHolder, memberInfo, (int)id)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return memberInfo.GetValueTypeFromMemberInfo().IsAssignableFrom(typeof(UnityEngine.Object));
        }

        private int GetIdFromObject(object memberHolder, MemberInfo memberInfo)
        {
            var obj = (Object)memberInfo.GetValueFromMemberInfo(memberHolder);
            var id = IdFactory.Instance.GetId(obj);
            Debug.Log($"Retrieved id {id} of obj {obj.name} of type {obj.GetType()}");
            return id;
        }

        private void SetObjectFromId(object memberHolder, MemberInfo memberInfo, int id)
        {
            if (ResourceRepository.Instance.TryGetObject(id, memberInfo.GetValueTypeFromMemberInfo(), out var obj))
            {
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
                Debug.Log($"Sat object from id {id} to {obj.name} using {nameof(ResourceRepository)}");
            }
            else if (ReferenceRepository.Instance.TryGetObject(id, memberInfo.GetValueTypeFromMemberInfo(), out obj))
            {
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
                Debug.Log($"Sat object from id {id} to {obj.name} using {nameof(ReferenceRepository)}");
            }
            else
            {
                memberInfo.SetValueFromMemberInfo(memberHolder, null);
            }

        }
    }
}
