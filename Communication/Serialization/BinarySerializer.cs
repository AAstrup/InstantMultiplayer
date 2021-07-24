using Communication.Serialization.Surrogates.Implementations;
using InstantMultiplayer.Communication.Serialization.Surrogates;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace InstantMultiplayer.Communication.Serialization
{
    public class BinarySerializer
    {
        private readonly static BinaryFormatter _formatter = new BinaryFormatter();

        public BinarySerializer()
        {
            Initiliaze();

        }

        public byte[] Serialize(object obj)
        {
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, obj);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        public object Deserialize(byte[] bytes)
        {
            return Deserialize(new MemoryStream(bytes));
        }

        public object Deserialize(Stream stream)
        {
            return _formatter.Deserialize(stream);
        }

        private void Initiliaze()
        {
            var surrogateSelector = new SurrogateSelector();
            foreach (var surrogateDefinition in DefaultSurrogateDefinitions())
                surrogateSelector.AddSurrogateDefinition(surrogateDefinition);
            _formatter.SurrogateSelector = surrogateSelector;
        }

        public static ISurrogateDefinition[] DefaultSurrogateDefinitions()
        {
            return new ISurrogateDefinition[]{
                new Vector2Surrogate(),
                new Vector3Surrogate(),
                new QuaternionSurrogate()
            };
        }
    }
}
