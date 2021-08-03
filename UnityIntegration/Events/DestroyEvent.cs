namespace InstantMultiplayer.UnityIntegration.Events
{
    public class DestroyEvent: AEvent
    {
        public int SynchronizerId;

        public override EventType Type => EventType.Destroy;
    }
}
