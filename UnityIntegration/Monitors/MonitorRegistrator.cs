using InstantMultiplayer.Synchronization.Monitored;
using InstantMultiplayer.UnityIntegration.Monitors.Components;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Monitors
{
    class MonitorRegistrator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoadRuntimeMethod()
        {
            MonitorFactory.RegisterComponentProvider(new ASyncMemberInterpolatorBaseMonitorProvider());
        }
    }
}
