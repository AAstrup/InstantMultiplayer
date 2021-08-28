using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class TransformMonitorProvider : AGenericComponentMonitorProvider<Transform>
    {
        public override string[] MemberInfoNames { get; } = new string[]
        {
            nameof(Transform.localPosition),
            nameof(Transform.localRotation),
            nameof(Transform.localScale)
        };
    }
}
