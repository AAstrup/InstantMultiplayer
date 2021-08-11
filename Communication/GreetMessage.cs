using System;

namespace Communication
{
    [Serializable]
    public class GreetMessage
    {
        public int LocalId;
        public DateTime InitialSyncTimestamp;
    }
}
