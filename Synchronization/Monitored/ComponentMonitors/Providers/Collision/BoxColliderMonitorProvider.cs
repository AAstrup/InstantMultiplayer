using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers.Collision
{
    public sealed class BoxColliderMonitorProvider : AGenericComponentMonitorProvider<BoxCollider>
    {
        public override string[] MemberInfoNames { get; } = new string[]
        {
            nameof(BoxCollider.isTrigger),
            //IMPORTANT: Do not change to material from sharedmaterial; odd bug where retrieving material in editor will unassign the material from the BoxCollider...
            nameof(BoxCollider.sharedMaterial),
            nameof(BoxCollider.center),
            nameof(BoxCollider.size),
        };
    }
}
