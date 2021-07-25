namespace Synchronization.Extensions
{
    public static class ObjectExtensions
    {
        public static string NonInstanceName(this UnityEngine.Object obj)
        {
            return NonInstanceName(obj.name);
        }

        public static string NonInstanceName(string name)
        {
            return name.EndsWith(" (Instance)") ? 
                name.Substring(0, name.Length - " (Instance)".Length) 
                : name.EndsWith(" Instance") ?
                name.Substring(0, name.Length - " Instance".Length)
                : name;
        }
    }
}
