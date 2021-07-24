using InstantMultiplayer.Synchronization.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class GenericUnityObjectMemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue;

        public MemberMonitor GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new MemberMonitor(
                () => GetIIDFromInfo(memberHolder, memberInfo),
                (iid) => SetObjectFromIID(memberHolder, memberInfo, (int)iid)
            );
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return _applicableTypes.Contains(memberInfo.DeclaringType);
        }

        private static readonly List<Type> _applicableTypes = new List<Type>()
        {
            typeof(Texture2D),
            typeof(Texture)
        };

        private int GetIIDFromInfo(object memberHolder, MemberInfo memberInfo)
        {
            /*var obj = (Object)memberInfo.GetValueFromMemberInfo(memberHolder);
            var iid = obj.GetInstanceID();
            Debug.Log("Got iid " + iid + " from " + obj.name);
            return iid;*/
            return 0;
        }

        private void SetObjectFromIID(object memberHolder, MemberInfo memberInfo, int iid)
        {
            /*var obj = (Object)UnityObjectHelper.FindObjectFromInstanceID(iid);
            Debug.Log("Got obj " + obj.name + " from " + iid);
            memberInfo.SetValueFromMemberInfo(memberHolder, obj);*/
        }
    }
}
