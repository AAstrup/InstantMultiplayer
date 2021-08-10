namespace InstantMultiplayer.UnityIntegration.Events
{
    public class DestroyEvent: AEvent
    {
        public int SynchronizerId;
        public int ClientFilter;

        public override EventType Type => EventType.Destroy;
    }
}
