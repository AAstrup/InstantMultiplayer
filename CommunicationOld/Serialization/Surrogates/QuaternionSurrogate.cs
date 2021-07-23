using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace InstantMultiplayer.Communication.Serialization.Surrogates
{
    public class QuaternionSurrogate : ISurrogateDefinition
    {
        public Type Type => typeof(Quaternion);
        public StreamingContext StreamingContext => new StreamingContext(StreamingContextStates.All);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quaternion val = (Quaternion)obj;
            info.AddValue("x", val.x);
            info.AddValue("y", val.y);
            info.AddValue("z", val.z);
            info.AddValue("w", val.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion val = (Quaternion)obj;
            val.x = (float)info.GetValue("x", typeof(float));
            val.y = (float)info.GetValue("y", typeof(float));
            val.z = (float)info.GetValue("z", typeof(float));
            val.w = (float)info.GetValue("w", typeof(float));
            return val;
        }
    }
}
