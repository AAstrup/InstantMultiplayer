using InstantMultiplayer.Initialization;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration
{
    internal class Initialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoadRuntimeMethod()
        {
            TypeInitializer.Instance.Initialize();
        }
    }
}
