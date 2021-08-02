using System.Reflection;

namespace InstantMultiplayer.Synchronization
{
    public class UnityObjectHelper
    {
        public static UnityObjectHelper Instance => _instance ?? (_instance = new UnityObjectHelper());

        private static UnityObjectHelper _instance;
        private MethodInfo _findObjectFromIIDMethod;

        public static object FindObjectFromInstanceID(int iid)
        {
            return Instance._findObjectFromIIDMethod.Invoke(null, new object[] { iid });
        }

        private UnityObjectHelper()
        {
            _findObjectFromIIDMethod = typeof(UnityEngine.Object)
                .GetMethod("FindObjectFromInstanceID", BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}
