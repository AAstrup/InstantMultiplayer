using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.Synchronization.Identification;
using InstantMultiplayer.Synchronization.Objects;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class UnityObjectMemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue;

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new RichMemberMonitor<int?, UnityEngine.Object>(
                memberInfo.Name,
                () => GetIdFromObject(memberHolder, memberInfo),
                () => (Object)memberInfo.GetValueFromMemberInfo(memberHolder),
                (id) => SetObjectFromId(memberHolder, memberInfo, id)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return memberInfo.GetValueTypeFromMemberInfo().IsAssignableFrom(typeof(UnityEngine.Object));
        }

        private int? GetIdFromObject(object memberHolder, MemberInfo memberInfo)
        {
            var obj = (Object)memberInfo.GetValueFromMemberInfo(memberHolder);
            if (obj == null)
                return null;
            var id = IdFactory.Instance.GetId(obj);
            Debug.Log($"Retrieved id {id} of obj {obj.name} of type {obj.GetType()}");
            return id;
        }

        private void SetObjectFromId(object memberHolder, MemberInfo memberInfo, int? idOption)
        {
            if(!idOption.HasValue)
            {
                memberInfo.SetValueFromMemberInfo(memberHolder, null);
                return;
            }

            var id = idOption.Value;
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
