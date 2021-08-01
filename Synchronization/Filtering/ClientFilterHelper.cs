namespace InstantMultiplayer.Synchronization.Filtering
{
    public static class ClientFilterHelper
    {
        public static bool ClientIncluded(int clientFilter, int clientId)
        {
            return (clientFilter & (1 << clientId)) > 0;
        }
    }
}
