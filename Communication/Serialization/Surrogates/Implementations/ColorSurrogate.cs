using InstantMultiplayer.Communication.Serialization.Surrogates;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Communication.Serialization.Surrogates.Implementations
{
    public class ColorSurrogate : ISurrogateDefinition
    {
        public Type Type => typeof(Color);
        public StreamingContext StreamingContext => new StreamingContext(StreamingContextStates.All);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color val = (Color)obj;
            info.AddValue("r", val.r);
            info.AddValue("g", val.g);
            info.AddValue("b", val.b);
            info.AddValue("a", val.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color val = (Color)obj;
            val.r = (float)info.GetValue("r", typeof(float));
            val.g = (float)info.GetValue("g", typeof(float));
            val.b = (float)info.GetValue("b", typeof(float));
            val.a = (float)info.GetValue("a", typeof(float));
            return val;
        }
    }
}
