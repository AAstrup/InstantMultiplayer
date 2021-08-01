using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation
{
    public abstract class ASyncMemberInterpolatorBase : MonoBehaviour, IDeltaMemberHandler, IForeignComponent
    {
        public Component Component;
        [HideInInspector]
        public int SelectedIndex;

        public bool LocalLerping;
        public float LocalLerpScale;

        public abstract Type GenericType { get; }

        public bool ForeignOnly => true;

        protected AMemberMonitorBase _memberMonitorBase;

        public ComponentMonitor ComponentMonitorSelect(IEnumerable<ComponentMonitor> componentMonitors)
        {
            return componentMonitors.FirstOrDefault(m => m.MonitoredInstance == Component);
        }
        public AMemberMonitorBase MemberMonitorSelector(AMemberMonitorBase[] memberMonitorBases)
        {
            _memberMonitorBase = memberMonitorBases[SelectedIndex];
            return _memberMonitorBase;
        }

        public abstract void HandleDeltaMember(DeltaMember deltaMember);
    }
}
