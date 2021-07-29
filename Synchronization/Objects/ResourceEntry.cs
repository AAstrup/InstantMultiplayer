using System;

namespace InstantMultiplayer.Synchronization.Objects
{
    [Serializable]
    public class ResourceEntry
    {
        public string Name;
        public int Id;
        public string TypeName;
        public string Path;

        public override string ToString()
        {
            return $"{Name} with id {Id} of {TypeName} at {Path}";
        }
    }
}
