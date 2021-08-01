using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Synchronization
{
    [Serializable]
    public class SyncInstantiationEventMessage: ASyncEventMessage
    {
        public int PrefabId;
        public int SynchronizerId;
    }
}
