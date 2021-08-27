using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Events
{
    public class InvocationEvent: AEvent
    {
        public int ClientFilter;
        public int SynchronizerId;
        public Component Component;
        public string MethodName;
        public object[] Arguments;

        public override EventType Type => EventType.Invocation;
    }
}
