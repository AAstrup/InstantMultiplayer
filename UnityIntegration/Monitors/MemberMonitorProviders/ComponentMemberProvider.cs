using InstantMultiplayer.Synchronization.Extensions;
using InstantMultiplayer.UnityIntegration;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public class ComponentMemberProvider : IMemberMonitorProvider
    {
        public int Precedence => int.MaxValue;

        public AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo)
        {
            return new RichMemberMonitor<ComponentMemberMonitorDTO, GameObject>(memberInfo.Name,
                    () => GetDTO(memberHolder, memberInfo),
                    () => (GameObject)memberInfo.GetValueFromMemberInfo(memberHolder),
                    (v) => SetCompFromDTO(memberHolder, memberInfo, v));
        }

        public bool IsApplicable(object memberHolder, MemberInfo memberInfo)
        {
            return typeof(UnityEngine.Component).IsAssignableFrom(memberInfo.GetValueTypeFromMemberInfo());
        }

        private ComponentMemberMonitorDTO GetDTO(object memberHolder, MemberInfo memberInfo)
        {
            var comp = (Component)memberInfo.GetValueFromMemberInfo(memberHolder);
            var iid = comp.gameObject.GetInstanceID();
            if (SynchronizeStore.Instance.TryGetFromIID(iid, out var sync))
            {
                var type = memberInfo.GetValueTypeFromMemberInfo();
                var comps = comp.gameObject.GetComponents(type);
                var index = Array.IndexOf(comps, comp);
                if(ComponentMapper.TryGetCIDFromType(type, out var cid))
                    return new ComponentMemberMonitorDTO
                    {
                        SyncId = sync.SynchronizerId,
                        TypeId = cid,
                        Index = (byte)index
                    };
            }
            return null;
        }

        private void SetCompFromDTO(object memberHolder, MemberInfo memberInfo, ComponentMemberMonitorDTO dto)
        {
            if (dto != null && SynchronizeStore.Instance.TryGet(dto.SyncId, out var synchronizer))
            {
                if (ComponentMapper.TryGetTypeFromCID(dto.TypeId, out var type))
                {
                    var comps = synchronizer.gameObject.GetComponents(type);
                    var comp = dto.Index >= 0 && comps.Length < dto.Index ? comps[dto.Index] : comps.FirstOrDefault();
                    if (comp != null)
                    {
                        memberInfo.SetValueFromMemberInfo(memberHolder, comp);
                        return;
                    }
                }
            }
            memberInfo.SetValueFromMemberInfo(memberHolder, null);
        }

        [Serializable]
        private class ComponentMemberMonitorDTO
        {
            public int SyncId;
            public int TypeId;
            public byte Index;
        }
    }
}
