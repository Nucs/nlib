using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using nucs.Automation.Mirror;

namespace nucs.Automation.Imaging {
    public static class ImageSearching {
        public static Rectangle WaitUntil(this ImageSource<DirectBitmap> source, DirectBitmap small, int toleranceprecentage, ImageSearchingOptions opts = null) {
            opts = opts ?? ImageSearchingOptions.SingleMultithreaded;
            opts.NumberOfResults = 1;
            opts.AllowMultithreading = true;

            List<Rectangle> ret;
            do {
                using (var big = source.FetchFresh)
                    ret = big.FindSubimages(small, toleranceprecentage, ImageSearchingOptions.SingleMultithreaded);
            } while (ret == null || ret.Count == 0);
            return ret[0];
        }

        public static bool SubimageExist(this ImageSource<DirectBitmap> source, DirectBitmap small, int toleranceprecentage, bool forceupdate = false, ImageSearchingOptions opts = null) {
            opts = opts ?? ImageSearchingOptions.SingleMultithreaded;
            opts.NumberOfResults = 1;
            opts.AllowMultithreading = true;

            using (var big = forceupdate ? source.FetchFresh : source.Fetch)
                return (big.FindSubimages(small, toleranceprecentage, ImageSearchingOptions.SingleMultithreaded)?.Count ?? 0) > 0;
        }
        public static List<Rectangle> Subimages(this ImageSource<DirectBitmap> source, DirectBitmap small, int toleranceprecentage, bool forceupdate = false, ImageSearchingOptions opts = null) {
            opts = opts ?? ImageSearchingOptions.Default;
            using (var big = forceupdate ? source.FetchFresh : source.Fetch)
                return (big.FindSubimages(small, toleranceprecentage, ImageSearchingOptions.SingleMultithreaded));
        }

        /// <summary>
        ///     Finds a single subimage, null if not found.
        /// </summary>
        public static Rectangle? SingleSubimage(this ImageSource<DirectBitmap> source, DirectBitmap small, int toleranceprecentage, bool forceupdate = false, ImageSearchingOptions opts = null) {
            opts = opts ?? ImageSearchingOptions.SingleMultithreaded;
            opts.NumberOfResults = 1;
            opts.AllowMultithreading = true;
            var ret = Subimages(source, small, toleranceprecentage, forceupdate, opts)?.SingleOrDefault();
            return ret==Rectangle.Empty ? null : ret;
        }

        public static ImageSource<DirectBitmap> ToImageSource(this Window wnd, uint cachetime = 100) {
            return new ImageSource<DirectBitmap>(wnd.CaptureImage, cachetime);
        }
    }
}