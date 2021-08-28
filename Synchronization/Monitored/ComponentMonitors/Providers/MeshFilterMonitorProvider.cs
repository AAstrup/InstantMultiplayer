using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class MeshFilterMonitorProvider : AGenericComponentMonitorProvider<MeshFilter>
    {
        public override string[] MemberInfoNames { get; } = new string[]
        {
            nameof(MeshFilter.sharedMesh)
        };
    }
}
