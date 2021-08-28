using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers.Collision
{
    public sealed class SphereMonitorProvider : AGenericComponentMonitorProvider<SphereCollider>
    {
        public override string[] MemberInfoNames { get; } = new string[]
        {
            nameof(SphereCollider.isTrigger),
            //IMPORTANT: Do not change to material from sharedmaterial; odd bug where retrieving material in editor will unassign the material from the BoxCollider...
            nameof(SphereCollider.sharedMaterial),
            nameof(SphereCollider.center),
            nameof(SphereCollider.radius),
        };
    }
}
