namespace InstantMultiplayer.UnityIntegration.Events
{
    public class InstantiationEvent: AEvent
    {
        public int PrefabId;
        public int SynchronizerId;
        public int ClientFilter;

        public override EventType Type => EventType.Instantiation;
    }
}
