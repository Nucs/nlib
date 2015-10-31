using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TFer.Serialization {

    public static class BinarySerialization  {

        /// <summary>
        ///     Serializes data to bytes array through BinaryFormatter
        /// </summary>
        public static byte[] SerializeBinary(this object o) {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, o);
            var data = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(data, 0, data.Length);
            ms.Dispose();

            return data;
        }

        /// <summary>
        ///     Deserializes data to bytes array through BinaryFormatter
        /// </summary>
        public static T DeserializeBinary<T>(this byte[] data) {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream(data);

            var o = bf.Deserialize(ms);
            ms.Dispose();

            return (T)o;
        }
    }
}
