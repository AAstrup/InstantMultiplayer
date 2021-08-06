using System;

namespace InstantMultiplayer.Synchronization.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSystemPrimitive(this Type type)
        {
            return type.IsValueType && type.Namespace == "System";
        }

        public static bool IsUnityPrimitive(this Type type)
        {
            return type.IsValueType && type.Namespace == "UnityEngine";
        }
    }
}
