using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.Synchronization.Identification;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors.RichMemberMonitors;
using InstantMultiplayer.Synchronization.Objects;
using System;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class UnityObjectMemberProvider : IMemberMonitorProvider
    {
        private readonly Type _memberInfoType;

        // Used by monitor default
        public UnityObjectMemberProvider() { }

        public UnityObjectMemberProvider(Type memberInfoType)
        {
            _memberInfoType = memberInfoType;
        }

        public int Precedence => int.MaxValue;

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new RichMemberMonitor<int?, UnityEngine.Object>(
                memberInfo.Name,
                () => GetIdFromObject(memberHolder, memberInfo), 
                () => (UnityEngine.Object)memberInfo.GetValueFromMemberInfo(memberHolder),
                (id) => SetObjectFromId(memberHolder, memberInfo, id)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(memberInfo.GetValueTypeFromMemberInfo());
        }

        private int? GetIdFromObject(object memberHolder, MemberInfo memberInfo)
        {
            var obj = (UnityEngine.Object)memberInfo.GetValueFromMemberInfo(memberHolder);
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
            var memberInfoType = _memberInfoType ?? memberInfo.GetValueTypeFromMemberInfo();
            if (ResourceRepository.Instance.TryGetObject(id, memberInfoType, out var obj))
            {
                memberInfo.SetValueFromMemberInfo(memberHolder, obj);
                Debug.Log($"Sat object from id {id} to {obj.name} using {nameof(ResourceRepository)}");
            }
            else if (ReferenceRepository.Instance.TryGetObject(id, memberInfoType, out obj))
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
