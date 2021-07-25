using System;

namespace Synchronization.Objects
{
    [Serializable]
    public class ResourceEntry
    {
        public string Name;
        public string TypeName;
        public string Path;

        public override string ToString()
        {
            return $"{Name} of {TypeName} at {Path}";
        }
    }
}
