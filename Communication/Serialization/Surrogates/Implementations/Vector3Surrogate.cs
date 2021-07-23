using InstantMultiplayer.Communication.Serialization.Surrogates;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Communication.Serialization.Surrogates.Implementations
{
    public class Vector3Surrogate : ISurrogateDefinition
    {
        public Type Type => typeof(Vector3);
        public StreamingContext StreamingContext => new StreamingContext(StreamingContextStates.All);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 val = (Vector3)obj;
            info.AddValue("x", val.x);
            info.AddValue("y", val.y);
            info.AddValue("z", val.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 val = (Vector3)obj;
            val.x = (float)info.GetValue("x", typeof(float));
            val.y = (float)info.GetValue("y", typeof(float));
            val.z = (float)info.GetValue("z", typeof(float));
            return val;
        }
    }
}
