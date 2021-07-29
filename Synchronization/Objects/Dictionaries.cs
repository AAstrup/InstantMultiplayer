using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMultiplayer.Synchronization.Objects
{
    public class Dictionaries<T>
    {
        public Dictionary<string, T> Names;
        public Dictionary<int, T> Ids;

        public Dictionaries()
        {
            Names = new Dictionary<string, T>();
            Ids = new Dictionary<int, T>();
        }
    }
}
