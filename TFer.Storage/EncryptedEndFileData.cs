using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using nucs.Cryptography;
using TFer.Serialization;

namespace TFer.EndData {

    /// <summary>
    ///     Allows storage of data in the end of a file.
    /// </summary>
    public static class EncryptedEndFileData {
        public static bool HasEncryptedEndData(this FileInfo fi) {
            try {
                using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
                    fs.Position = fs.Length - sizeof(long);
                    var endfile_pos = BitConverter.ToInt64(fs.ReadBytes(8), 0);
                    fs.Position = fs.Length - endfile_pos;
                    var data = fs.ReadBytes(endfile_pos - sizeof (long));
                    var val = Encoding.UTF8.GetString(data).Trim();
                    return (val.Length % 4 == 0) && Regex.IsMatch(val, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None) && Convert.FromBase64String(val)!=null;
                }
            } catch {
                return false;
            }
        }

        public static string ComputeMD5(this FileInfo fi) {
            using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
                using (var md5 = System.Security.Cryptography.MD5.Create()) {
                    return BitConverter.ToString(md5.ComputeHash(fs.ReadBytes(fs.Length))).Replace("-", "").ToLower();
                }
            }
        }

        public static void WriteEndData(this FileInfo fi, object data, string pass, ICrypter crypter = null) {
            crypter = crypter ?? new RijndaelCrypter();
            using (var fs = new FileStream(fi.FullName, FileMode.Append, FileAccess.Write)) {
                var obj = data.SerializeBinary(pass,crypter); //gets encrypted serialized data.

                var end_jumpsize = BitConverter.GetBytes((long) obj.Length + sizeof (long)); //size of the jump to the start of the object.

                var output = obj.Concat(end_jumpsize).ToArray(); //concats the output
                fs.Position = fs.Length; //goes to the end of the file
                fs.Write(output, 0, output.Length); //writes it all.
            }
        }

        public static void WriteEndData(this Stream stream, object data, string pass, ICrypter crypter = null) {
            crypter = crypter ?? new RijndaelCrypter();

            var obj = data.SerializeBinary(pass, crypter); //gets encrypted serialized data.

            var end_jumpsize = BitConverter.GetBytes((long) obj.Length + sizeof (long)); //size of the jump to the start of the object.

            var output = obj.Concat(end_jumpsize).ToArray(); //concats the output
            stream.Position = stream.Length; //goes to the end of the file
            stream.Write(output, 0, output.Length); //writes it all.
        }

        public static ReadEndDataResult<T> ReadEndData<T>(this FileInfo fi, string pass, ICrypter crypter = null) {
            crypter = crypter ?? new RijndaelCrypter();
            var redr = new ReadEndDataResult<T>() {Crypter = crypter};
            try {
                using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read)) {
                    fs.Position = fs.Length - sizeof(long);
                    var endfile_pos = BitConverter.ToInt64(fs.ReadBytes(8), 0);
                    fs.Position = fs.Length - endfile_pos;
                    redr.Result = fs.ReadBytes(endfile_pos - sizeof(long)).DeserializeBinary<T>(pass, crypter);
                }
            } catch (Exception e) {
                redr.Exception = e;
            }
            return redr;
        }

        public static void DeleteEndData(FileInfo fi) {
            if (!HasEncryptedEndData(fi))
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
            public ICrypter Crypter;
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
        private static Stream ToMemoryStream(this string @this)
        {
            Encoding encoding = Activator.CreateInstance<ASCIIEncoding>();
            return new MemoryStream(encoding.GetBytes(@this));
        }
    }
}