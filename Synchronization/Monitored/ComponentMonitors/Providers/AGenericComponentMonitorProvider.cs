using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public abstract class AGenericComponentMonitorProvider<T> : AComponentMonitorProvider<T> where T : Component
    {
        public override MemberInfo[] MemberInfos { 
            get { 
                return _memberInfo ?? (_memberInfo = MemberInfoNames
                    .Select(m => typeof(T).GetProperty(m))
                    .ToArray());
            } 
        }
        private MemberInfo[] _memberInfo;

        public abstract string[] MemberInfoNames { get; }
    }
}
