using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.ComponentMonitors.Providers
{
    public sealed class SpriteRendererMonitorProvider : AGenericComponentMonitorProvider<SpriteRenderer>
    {
        public override string[] MemberInfoNames { get; } = new string[]
        {
            nameof(SpriteRenderer.sprite),
            nameof(SpriteRenderer.color)
        };
    }
}
