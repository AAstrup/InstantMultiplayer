using System;

namespace Communication.Synchronization.Events
{
    [Serializable]
    public class SyncDestroyEventMessage: ASyncEventMessage
    {
        public int SynchronizerId;
    }
}
