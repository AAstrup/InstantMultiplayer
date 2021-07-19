using System.Reflection;
using UnityEngine;

namespace Assets
{
    public class UnityObjectHelper
    {
        public static UnityObjectHelper Instance => _instance ?? (_instance = new UnityObjectHelper());

        private static UnityObjectHelper _instance;
        private MethodInfo _findObjectFromIIDMethod;

        public static Object FindObjectFromInstanceID(int iid)
        {
            return (Object)Instance._findObjectFromIIDMethod.Invoke(null, new object[] { iid });
        }

        private UnityObjectHelper()
        {
            _findObjectFromIIDMethod = typeof(Object)
                .GetMethod("FindObjectFromInstanceID", BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}
