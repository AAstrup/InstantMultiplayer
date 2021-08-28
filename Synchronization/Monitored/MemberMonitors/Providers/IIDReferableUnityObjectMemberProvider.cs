using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class IIDReferableUnityObjectMemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue - 1;

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new MemberMonitor<int>(
                memberInfo.Name,
                () => GetIIDFromInfo(memberHolder, memberInfo),
                (int iid) => SetObjectFromIID(memberHolder, memberInfo, iid)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            //Disabled
            return false; //_applicableTypes.Contains(memberInfo.GetValueTypeFromMemberInfo());
        }

        private static readonly List<Type> _applicableTypes = new List<Type>()
        {
            typeof(Texture2D),
            typeof(Texture)
        };

        private int GetIIDFromInfo(object memberHolder, MemberInfo memberInfo)
        {
            var obj = (UnityEngine.Object)memberInfo.GetValueFromMemberInfo(memberHolder);
            var iid = obj.GetInstanceID();
            return iid;
        }

        private void SetObjectFromIID(object memberHolder, MemberInfo memberInfo, int iid)
        {
            var obj = (UnityEngine.Object)UnityObjectHelper.FindObjectFromInstanceID(iid);
            Debug.Log("Got obj " + obj.name + " from " + iid);
            memberInfo.SetValueFromMemberInfo(memberHolder, obj);
        }
    }
}
