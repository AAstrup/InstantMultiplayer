using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMultiplayer.UnityIntegration
{
    public static class SynchronizationConstants
    {
        public static List<Type> NonSynchronizeableComponentTypes = new List<Type>
        {
            typeof(Synchronizer),
            typeof(SyncClient)
        };
    }
}
