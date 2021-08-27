using System;

namespace Communication.Synchronization.Events
{
    [Serializable]
    public class SyncInvocationEventMessage : ASyncEventMessage
    {
        public int SynchronizerId;
        public int CompId;
        public string MethodName;
        public object[] Arguments;
    }
}
