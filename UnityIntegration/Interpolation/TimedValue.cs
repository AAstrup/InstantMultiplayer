namespace InstantMultiplayer.UnityIntegration.Interpolation
{
    public struct TimedValue<T>
    {
        public T Value;
        public float LocalTimeStamp;
        public float ForeignSyncTimestamp;
    }
}
