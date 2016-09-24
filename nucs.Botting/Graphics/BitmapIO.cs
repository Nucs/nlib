using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace nucs.Windows.Graphics {
    public static class BitmapIO {


        private static readonly string[] gdiplus_file_formats = { ".bmp", ".gif", ".exig", ".jpg", ".jpeg", ".png", ".tiff" };


        /// <summary>
        /// Loads a single bitmap into <see cref="FastBitmap"/> object. Use "~/dir/filename.bmp" to access the executing directory
        /// </summary>
        public static Bitmap LoadFromFile(string file) {
            file = file.Replace("~", Path.GetDirectoryName(Application.ExecutablePath)).Replace("/", @"\");
            if (File.Exists(file) == false)
                throw new IOException("File at "+file+" was not found.");
            return new Bitmap((Bitmap)Image.FromFile(file));
        }
        
        /// <summary>
        /// Loads multiple bitmaps into <see cref="FastBitmap"/> object. Use "~/dir/" to access the executing directory
        /// </summary>
        public static List<Bitmap> LoadFromDirectory(string directory) {
            directory = directory.Replace("~", Path.GetDirectoryName(Application.ExecutablePath)).Replace("/", @"\");
            return (from filepath in Directory.GetFiles(directory) where gdiplus_file_formats.Contains(Path.GetExtension(filepath)) select (Bitmap) Image.FromFile(filepath)).ToList();
        }

        /// <summary>
        /// Loads multiple bitmaps into <see cref="FastBitmap"/> object. Use "~/dir/" to access the executing directory
        /// </summary>
        public static Dictionary<string, Bitmap> LoadFromDirectoryToDic(string directory) {
            directory = directory.Replace("~", Path.GetDirectoryName(Application.ExecutablePath)).Replace("/", @"\");
            return Directory.GetFiles(directory).Where(filepath => gdiplus_file_formats.Contains(Path.GetExtension(filepath))).ToDictionary(Path.GetFileNameWithoutExtension, filepath => (Bitmap) Image.FromFile(filepath));
        }
    }
}