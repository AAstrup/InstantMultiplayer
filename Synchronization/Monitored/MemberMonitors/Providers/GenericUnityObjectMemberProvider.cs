using InstantMultiplayer.Synchronization.Extensions;
using System.Reflection;

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
            return memberInfo.DeclaringType.IsAssignableFrom(typeof(UnityEngine.Object));
        }

        private int GetIIDFromInfo(object memberHolder, MemberInfo memberInfo)
        {
            return ((UnityEngine.Object)memberInfo.GetValueFromMemberInfo(memberHolder)).GetInstanceID();
        }

        private void SetObjectFromIID(object memberHolder, MemberInfo memberInfo, int iid)
        {
            var obj = UnityObjectHelper.FindObjectFromInstanceID(iid);
            memberInfo.SetValueFromMemberInfo(memberHolder, obj);
        }
    }
}
