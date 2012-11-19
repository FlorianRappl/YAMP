using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
    internal class BinaryFormatter
    {
        internal void Serialize(Stream serializationStream, Object graph) { }

        internal object Deserialize(Stream serializationStream)
        {
            return null;
        }
    }
}