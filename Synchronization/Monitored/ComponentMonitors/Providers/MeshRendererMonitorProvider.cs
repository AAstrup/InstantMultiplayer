using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class MeshRendererMonitorProvider : AGenericComponentMonitorProvider<MeshRenderer>
    {
        public override string[] MemberInfoNames { get; } = new string[]
        {
            nameof(MeshRenderer.sharedMaterial)
        };
    }
}
