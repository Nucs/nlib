using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nucs.Windows.Processes;

namespace nucs.Windows.FileSystem
{
    /// <summary>
    ///     Gives various syncronized methods to a specific file.
    /// </summary>
    public class FilePointer {
        public string FilePath { get; private set; }

        public object Syncer { get; private set; }

        public FilePointer(string filepath) {
            FilePath = filepath;
            Syncer = new object();
        }

        /// <summary>
        ///     Checks if the file exists
        /// </summary>
        public bool Exists {
            get { return File.Exists(FilePath); }
        }

        /// <summary>
        ///     Checks if the directory exists
        /// </summary>
        public bool DirectoryExists() {
            return Directory.Exists(Path.GetDirectoryName(FilePath));
        }

        /// <summary>
        ///     Creates a blank file
        /// </summary>
        public void CreateBlank() {
            if (Directory.Exists(Path.GetDirectoryName(FilePath)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            if (Exists == false)
                lock (Syncer)
                    File.Create(FilePath);
        }

        public void SaveText(string text, bool append = false, Encoding encoding = null) {
            encoding = encoding ?? Encoding.UTF8;
            if (Directory.Exists(Path.GetDirectoryName(FilePath)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            lock (Syncer) {
                if (append)
                    text = File.ReadAllText(FilePath, encoding) + text;
                File.WriteAllText(FilePath, text, encoding);
            }
        }

        /// <summary>
        ///     Reads a text file.
        ///     If file doesn't exist, null is returned
        /// </summary>
        public string ReadText(Encoding encoding = null) {
            if (Exists == false)
                return null;
            lock (Syncer)
                return File.ReadAllText(FilePath, encoding ?? Encoding.UTF8);
        }

        /// <summary>
        ///     Opens the file.
        /// </summary>
        public void Start(string[] arguments = null) {
            //todo finish
        }

        /// <summary>
        ///     Searches all directories and file inside the <paramref name="directory"/> and 
        ///     returns a FilePointer to the first occurance, otherwise null;
        /// </summary>
        /// <param name="filename">The filename, with extension or without</param>
        /// <param name="directory">The directory to search, search will be performed to subdirectories</param>
        /// <param name="extension">Possible specification for the extension, for example "exe" or "doc" </param>
        public static FilePointer FindInDirectory(string directory, string filename, string extension=null) {
            var files = FileHelper.SearchFile(filename, directory, extension);
            if (files.Length == 0) return null;
            return new FilePointer(files[0]);
        }

        //todo Monitor file

    }
}
