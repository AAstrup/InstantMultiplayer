using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMultiplayer.Communication.Match
{
    [Serializable]
    public class MatchLoginRequest
    {
        public int id;
        public string name;
    }
}
