using System;
using System.Runtime.Serialization;

namespace InstantMultiplayer.Communication.Serialization.Surrogates
{
    public interface ISurrogateDefinition: ISerializationSurrogate
    {
        Type Type { get; }
        StreamingContext StreamingContext { get; }
    }
}
