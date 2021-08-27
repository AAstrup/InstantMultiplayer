using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.Synchronization.Monitored.ComponentMonitors;
using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Interpolation
{
    public abstract class ASyncMemberInterpolatorBase : MonoBehaviour, IDeltaMemberHandler, IDeltaMemberSuppressor, IForeignComponent
    {
        public Component Component;
        [HideInInspector]
        public int SelectedIndex;

        public bool LocalLerping;
        public float LocalLerpScale;

        public abstract Type GenericType { get; }

        public abstract bool IsMemberSuppressed { get; }


        protected AMemberMonitorBase _memberMonitorBase;

        public ComponentMonitor HandledComponentMonitor(IEnumerable<ComponentMonitor> componentMonitors)
        {
            return componentMonitors.FirstOrDefault(m => m.MonitoredInstance == Component);
        }
        public AMemberMonitorBase HandledMemberMonitor(ReadOnlyCollection<AMemberMonitorBase> memberMonitorBases)
        {
            _memberMonitorBase = memberMonitorBases[SelectedIndex];
            return _memberMonitorBase;
        }

        public abstract void HandleDeltaMember(DeltaMember deltaMember);

        public ComponentMonitor SuppressedComponentMonitor(IEnumerable<ComponentMonitor> componentMonitors)
        {
            return componentMonitors.FirstOrDefault(m => m.MonitoredInstance == Component);
        }

        public AMemberMonitorBase SuppressedMemberMonitor(ReadOnlyCollection<AMemberMonitorBase> memberMonitorBases)
        {
            _memberMonitorBase = memberMonitorBases[SelectedIndex];
            return _memberMonitorBase;
        }

        public bool ShouldSuppress(DeltaMember deltaMember)
        {
            return isActiveAndEnabled && IsMemberSuppressed;
        }

        public bool ShouldHandle(DeltaMember deltaMember)
        {
            return isActiveAndEnabled;
        }
    }
}
