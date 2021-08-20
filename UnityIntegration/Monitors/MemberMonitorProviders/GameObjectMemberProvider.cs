using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.UnityIntegration;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class GameObjectMemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue;

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new RichMemberMonitor<int?, GameObject>(memberInfo.Name,
                    () => GetGameObjectSyncId(memberHolder, memberInfo),
                    () => (GameObject)memberInfo.GetValueFromMemberInfo(memberHolder),
                    (v) => SetGameObjectFromSyncId(memberHolder, memberInfo, v));
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return typeof(UnityEngine.GameObject).IsAssignableFrom(memberInfo.GetValueTypeFromMemberInfo());
        }

        private int? GetGameObjectSyncId(object memberHolder, MemberInfo memberInfo)
        {
            var iid = ((GameObject)memberInfo.GetValueFromMemberInfo(memberHolder)).GetInstanceID();
            if (SynchronizeStore.Instance.TryGetFromIID(iid, out var sync))
                return sync.SynchronizerId;
            return null;
        }

        private void SetGameObjectFromSyncId(object memberHolder, MemberInfo memberInfo, int? syncId)
        {
            if (syncId.HasValue && SynchronizeStore.Instance.TryGet(syncId.Value, out var synchronizer))
            {
                memberInfo.SetValueFromMemberInfo(memberHolder, synchronizer.gameObject);
                return;
            }
            memberInfo.SetValueFromMemberInfo(memberHolder, null);
        }
    }
}
