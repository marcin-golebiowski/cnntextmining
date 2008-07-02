using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextMining.Model
{
    public class Serialization
    {
        public static byte[] Serialize(object @object, StreamingContextStates state)
        {
            MemoryStream memory = new MemoryStream();
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Context = new StreamingContext(state);
            serializer.Serialize(memory, @object);
            return memory.ToArray();
        }

        public static object Deserialize(byte[] @bytes, StreamingContextStates state)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Context = new StreamingContext(state);
            MemoryStream memory = new MemoryStream(@bytes);
            return serializer.Deserialize(memory);
        }
    }
}
