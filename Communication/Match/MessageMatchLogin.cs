using InstantMultiplayer.Communication;
using System;

namespace Communication.Match
{
    [Serializable]
    public class MessageMatchLogin : IMessage
    {
        public int id;
        public string name;
    }
}
