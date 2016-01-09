using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using nucs.EndData.Helpers;

namespace nucs.EndData {

    /// <summary>
    ///     Allows storage of data in the end of a file.
    /// </summary>
    public static class EndFileData {

        public static bool HasEndData(this FileInfo fi) {
            return ReadEndData<object>(fi).Exception == null;
        }

        public static string ComputeMD5(this FileInfo fi) {
            using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
                using (var md5 = System.Security.Cryptography.MD5.Create()) {
                    return BitConverter.ToString(md5.ComputeHash(fs.ReadBytes(fs.Length))).Replace("-", "").ToLower();
                }
            }
        }

        public static void WriteEndData(this FileInfo fi, object data) {
            using (var fs = new FileStream(fi.FullName, FileMode.Append, FileAccess.Write)) {
                //using (var fs = new FileStream(fi.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, data);
                var obj = stream.ToArray(); //gets serialized data.
                stream.Close();

                var end_jumpsize = BitConverter.GetBytes((long) obj.Length + sizeof (long)); //size of the jump to the start of the object.

                var output = obj.Concat(end_jumpsize).ToArray(); //concats the output
                fs.Position = fs.Length; //goes to the end of the file
                fs.Write(output, 0, output.Length); //writes it all.
            }
        }

        public static void WriteEndData(this Stream stream, object data) {
            //using (var fs = new FileStream(fi.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, data);
            var obj = memoryStream.ToArray(); //gets serialized data.
            memoryStream.Close();

            var end_jumpsize = BitConverter.GetBytes((long) obj.Length + sizeof (long)); //size of the jump to the start of the object.

            var output = obj.Concat(end_jumpsize).ToArray(); //concats the output
            stream.Position = stream.Length; //goes to the end of the file
            stream.Write(output, 0, output.Length); //writes it all.
        }

        public static ReadEndDataResult<T> ReadEndData<T>(this FileInfo fi) {
            var redr = new ReadEndDataResult<T>();
            try {
                using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
                    fs.Position = fs.Length - sizeof (long);
                    var endfile_pos = BitConverter.ToInt64(fs.ReadBytes(8), 0);
                    fs.Position = fs.Length - endfile_pos;
                    var data = fs.ReadBytes(endfile_pos - sizeof (long));
                    IFormatter formatter = new BinaryFormatter();
                    var res = formatter.Deserialize(data.ToMemoryStream());
                    if (typeof(T) != res.GetType()) throw new InvalidCastException("The deserialized object does not fit to the generic type T.");
                    redr.Result = (T) res;
                }
            } catch (Exception e) {
                redr.Exception = e;
            }
            return redr;
        }

        public static void DeleteEndData(this FileInfo fi) {
            if (!HasEndData(fi))
                return;
            using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.ReadWrite)) {
                fs.Position = fs.Length - sizeof (long);
                var endfile_pos = BitConverter.ToInt64(fs.ReadBytes(8), 0);
                fs.SetLength(fs.Length - endfile_pos);
            }
        }


        public class ReadEndDataResult<T> {
            public Exception Exception;
            public T Result;

            public bool Failed {
                get {return !Success; }
            }

            public bool Success {
                get { return Exception == null; }
            }

            public override string ToString() {
                return Result.ToString();
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ReadEndDataResult<T>) obj);
            }

            public override int GetHashCode() {
                return Result.GetHashCode();
            }
        }
    }
    /// ###
    /// <summary>Byte array extension.</summary>
    public static partial class ByteArrayExtension
    {
        /// <summary>
        ///     A byte[] extension method that converts the @this to a memory stream.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a MemoryStream.</returns>
        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }
    }
}