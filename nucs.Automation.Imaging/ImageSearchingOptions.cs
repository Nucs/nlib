namespace nucs.Automation.Imaging {
    public class ImageSearchingOptions {
        /// <summary>
        ///     The default settings
        /// </summary>
        public static ImageSearchingOptions Default => new ImageSearchingOptions();
        public static ImageSearchingOptions Multithreaded {
            get {
                var d = Default;
                d.AllowMultithreading = true;
                return d;
            }
        }
        /// <summary>
        ///     Default settings that returns a single result.
        /// </summary>
        public static ImageSearchingOptions Single {
            get {
                var d = Default;
                d.NumberOfResults = 1;
                return d;
            }
        }
        /// <summary>
        ///     Default settings that returns a single result searching with multithreading.
        /// </summary>
        public static ImageSearchingOptions SingleMultithreaded {
            get {
                var d = Single;

                d.AllowMultithreading = true;
                return d;
            }
        }

        /// <summary>
        ///     Settings when search is being done in all screens at the same time.
        /// </summary>
        public static ImageSearchingOptions Screenshot {
            get {
                var d = Default;
                d.IsSearchingInAllScreens = true;
                return d;
            }
        }

        /// <summary>
        ///     When search is being done in all screens at the same time. Default: false.
        /// </summary>
        public bool IsSearchingInAllScreens = false;

        /// <summary>
        ///     Will split the searching into smaller pieces spread between cores, Default: false
        /// </summary>
        public bool AllowMultithreading = false;
        /// <summary>
        ///     Number of threads that will be active in searching.
        /// </summary>
        public int Threads = 4;

        /// <summary>
        ///     Alpha pixels in the smaller image will be taken as right pixels: default: true!
        /// </summary>
        public bool IgnoreAlphaPixels = true;

        /// <summary>
        ///     Limit number of results, if -1, it will return all results. (default: -1).
        /// </summary>
        public int NumberOfResults { get; set; } = -1;
    }
}