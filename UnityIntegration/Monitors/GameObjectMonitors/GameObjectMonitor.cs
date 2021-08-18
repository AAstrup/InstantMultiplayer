using InstantMultiplayer.Synchronization.Monitored.MemberMonitors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Monitors.GameObjectMonitors
{
    public sealed class GameObjectMonitor
    {
        public readonly GameObject GameObject;
        public ReadOnlyCollection<AMemberMonitorBase> Members => Array.AsReadOnly(_members);

        private readonly AMemberMonitorBase[] _members;

        internal GameObjectMonitor(GameObject gameObject, AMemberMonitorBase[] fields)
        {
            GameObject = gameObject;
            _members = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public override string ToString()
        {
            return "Monitored: " + GameObject.name;
        }
    }
}
