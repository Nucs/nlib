using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using nucs.Cryptography;

namespace nucs.SystemCore.Serialization {
    public static class BinarySerializationExtensions {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializable"></param>
        /// <param name="rijpass">Keep null for not encrypting</param>

        public static byte[] Seriallize(this object serializable, string rijpass = null) {
            IFormatter formatter = new BinaryFormatter();
            var ms = new MemoryStream();
            formatter.Serialize(ms, serializable);
            var data = ms.ToArray();
            return rijpass == null
                ? data
                : new RijndaelEnhanced(rijpass).EncryptToBytes(data);
        }
        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="rijpass">Keep null for not encrypting</param>
        /// <returns></returns>
        public static T Deseriallize<T>(this byte[] data, string rijpass = null) {
            IFormatter formatter = new BinaryFormatter();
            return rijpass == null
                ? (T)formatter.Deserialize(data.ToMemoryStream())
                : (T)formatter.Deserialize(new RijndaelEnhanced(rijpass).DecryptToBytes(data).ToMemoryStream());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializable"></param>
        /// <param name="rij">Keep null for not encrypting</param>

        public static byte[] Seriallize(this object serializable, RijndaelEnhanced rij = null)
        {
            IFormatter formatter = new BinaryFormatter();
            var ms = new MemoryStream();
            formatter.Serialize(ms, serializable);
            var data = ms.ToArray();
            return rij == null
                ? data
                : rij.EncryptToBytes(data);
        }
        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="rij">Keep null for not encrypting</param>
        /// <returns></returns>
        public static T Deseriallize<T>(this byte[] data, RijndaelEnhanced rij = null)
        {
            IFormatter formatter = new BinaryFormatter();
            return rij == null
                ? (T)formatter.Deserialize(data.ToMemoryStream())
                : (T)formatter.Deserialize(rij.DecryptToBytes(data).ToMemoryStream());
        }
    }
}