#if (NET_3_5 || NET_3_0 || NET_2_0)

using System.IO;

namespace nucs.SystemCore {
    public static class StreamExt {
        // Only useful before .NET 4
        public static void CopyTo(this Stream input, Stream output) {
            byte[] buffer = new byte[16*1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0) {
                output.Write(buffer, 0, bytesRead);
            }
        }

        /// <summary>
        ///     Writes the memorystream to a file!
        /// </summary>
        public static void ToFile(this MemoryStream ms, FileInfo filepath) {ToFile(ms, filepath.ToString());}

        public static void ToFile(this MemoryStream ms, string filepath) {
            using (FileStream file = new FileStream(filepath, FileMode.Create, System.IO.FileAccess.Write)) {
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, (int)ms.Length);
                file.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

#endif