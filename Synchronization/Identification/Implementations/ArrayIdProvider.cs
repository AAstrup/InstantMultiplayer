using System;

namespace Synchronization.HashCodes.Implementations
{
    public class ArrayIdProvider
    {
        public Type Type => typeof(Array);

        public int GetHashCode(object obj)
        {
            throw new Exception("Doesn't work");
            unchecked
            {
                var h = 0;
                var arr = (Array)obj;
                foreach (var entry in arr)
                    h += 23 * IdFactory.Instance.GetId(entry);
                return h;
            }
        }
    }
}
