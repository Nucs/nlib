using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using nucs.Automation.Imaging.Helpers;

namespace nucs.Automation.Imaging {
    /// <summary>
    ///     Same as DirectBitmap, automatically constructs AlphaMap.
    /// </summary>
    public class AlphaDirectBitmap : DirectBitmap {
        #region Static

        /// <summary>
        ///     Loads a DirectBitmap from a file into memory.
        /// </summary>
        /// <param name="file">The file to load</param>
        public new static AlphaDirectBitmap FromFile(FileInfo file, bool reduant = false) {
            return FromFile(file.FullName);
        }

        /// <summary>
        ///     Loads a DirectBitmap from a file into memory.
        /// </summary>
        /// <param name="file">The file to load</param>
        public new static AlphaDirectBitmap FromFile(string file, bool reduant = false) {
            using (var bmp = Image.FromFile(file)) {
                return new AlphaDirectBitmap(bmp as Bitmap ?? new Bitmap(bmp));
            }
        }

        /// <summary>
        ///     Loads a DirectBitmap from a file into memory.
        /// </summary>
        /// <param name="file">The file to load</param>
        public static AlphaDirectBitmap FromFile(FileInfo file) {
            return FromFile(file.FullName);
        }

        /// <summary>
        ///     Loads a DirectBitmap from a file into memory.
        /// </summary>
        /// <param name="file">The file to load</param>
        public static AlphaDirectBitmap FromFile(string file) {
            using (var bmp = Image.FromFile(file)) {
                return new AlphaDirectBitmap(bmp as Bitmap ?? new Bitmap(bmp));
            }
        }

        #endregion

        public AlphaDirectBitmap(int width, int height, PixelFormat format) : base(width, height, format) {
            Construct();
        }

        public AlphaDirectBitmap(Image img) : base(img) {
            Construct();
        }

        public AlphaDirectBitmap(Bitmap bmp) : base(bmp) {
            Construct();
        }

        /// <summary>
        ///     Initialize a DirectBitmap from embedded resource, part of the name will be enough to find it.
        /// </summary>
        /// <param name="embeddedresource_name"></param>
        public AlphaDirectBitmap(string embeddedresource_name) : base(embeddedresource_name) {
            Construct();
        }

        private unsafe void Construct() {
            Map = new AlphaMap(this);
        }
    }
}