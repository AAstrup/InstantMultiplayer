using System.Runtime.Serialization;

namespace InstantMultiplayer.Communication.Serialization.Surrogates
{
    public static class SurrogateSelectorExtensions
    {
        public static void AddSurrogateDefinition(this SurrogateSelector selector, ISurrogateDefinition surrogateDefinition)
        {
            selector.AddSurrogate(surrogateDefinition.Type, surrogateDefinition.StreamingContext, surrogateDefinition);
        }
    }
}
