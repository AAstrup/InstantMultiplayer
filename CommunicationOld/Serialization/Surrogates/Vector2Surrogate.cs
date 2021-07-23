using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace InstantMultiplayer.Communication.Serialization.Surrogates
{
    public class Vector2Surrogate : ISurrogateDefinition
    {
        public Type Type => typeof(Vector2);
        public StreamingContext StreamingContext => new StreamingContext(StreamingContextStates.All);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2 val = (Vector2)obj;
            info.AddValue("x", val.x);
            info.AddValue("y", val.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 val = (Vector2)obj;
            val.x = (float)info.GetValue("x", typeof(float));
            val.y = (float)info.GetValue("y", typeof(float));
            return val;
        }
    }
}
