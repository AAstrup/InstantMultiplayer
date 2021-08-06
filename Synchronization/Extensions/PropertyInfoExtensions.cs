using System.Reflection;

namespace InstantMultiplayer.Synchronization.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsPublicGetSetProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetSetMethod(true).IsPublic;
        }
    }
}
