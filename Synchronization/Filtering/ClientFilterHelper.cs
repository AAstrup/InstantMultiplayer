using System.Collections.Generic;

namespace InstantMultiplayer.Synchronization.Filtering
{
    public static class ClientFilterHelper
    {
        public static bool ClientIncluded(int clientFilter, int clientId)
        {
            return (clientFilter & (1 << clientId)) > 0;
        }

        public static int FilterFromClientId(int clientId)
        {
            return 1 << clientId;
        }

        public static int FilterFromClientIds(IEnumerable<int> clientIds)
        {
            var filter = 0;
            foreach (var clientId in clientIds)
                filter |= 1 << clientId;
            return filter;
        }
    }
}
