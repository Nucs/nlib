using System.IO;

namespace nucs.EndData.Helpers {
    internal static class StreamHelper {
        public static byte[] ReadBytes(this Stream stream, long count) {
            var arr = new byte[count];
            stream.Read(arr, 0, arr.Length);
            return arr;
        }
        public static byte[] ReadAll(this Stream stream) {
            var arr = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(arr, 0, arr.Length);
            return arr;
        }
    }
}