using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMultiplayer.UnityIntegration.Extensions
{
    public static class QueueExtensions
    {
        public static bool TryDequeue<T>(this Queue<T> queue, out T value)
        {
            if(queue.Count > 0)
            {
                value = queue.Dequeue();
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
