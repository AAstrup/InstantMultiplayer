using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Synchronization.Events
{
    [Serializable]
    public class SyncInstantiationEventMessage: ASyncEventMessage
    {
        public int PrefabId;
        public int SynchronizerId;
        public float Timestamp;
    }
}
