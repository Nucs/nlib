using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using nucs.Cryptography;

namespace TFer.Serialization
{
    public static class EncryptedBinarySerialization  {

        public static byte[] SerializeBinary(this object o, string pass, ICrypter crypter = null) {
            crypter = crypter ?? new RijndaelCrypter();
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, o);
            var data = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(data, 0, data.Length);
            ms.Dispose();

            return crypter.EncryptToBytes(data, pass);
        }

        public static T DeserializeBinary<T>(this byte[] data, string pass, ICrypter crypter = null) {
            crypter = crypter ?? new RijndaelCrypter();
            data = crypter.DecryptToBytes(data, pass);
            var bf = new BinaryFormatter();
            var ms = new MemoryStream(data);

            var o = bf.Deserialize(ms);

            ms.Dispose();

            if (typeof(T) != o.GetType())
                throw new InvalidCastException("The deserialized object does not fit to the generic type T.");

            return (T)o;
        }
    }
}
