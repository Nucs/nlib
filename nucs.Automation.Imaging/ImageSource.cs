using System;
using System.Windows.Media;
using nucs.SystemCore;

namespace nucs.Automation.Imaging {
    /// <summary>
    ///     Caches the image for 100ms
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImageSource<T> : IImageSource<T> where T : DirectBitmap {
        public Cache<T> Factory;

        public virtual T Fetch => Factory.Object;

        protected ImageSource() {}

        /// <param name="factory"></param>
        /// <param name="imagevalidfor_milliseconds">For how long will the image be cached for. Recommanded: 100ms</param>
        public ImageSource(Func<T> factory, uint imagevalidfor_milliseconds) {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            Factory = new Cache<T>(factory, imagevalidfor_milliseconds, false);
        }

        public virtual T FetchFresh => Factory.ForceUpdate();
    }


    public interface IImageSource<T> where T : DirectBitmap {
        T Fetch { get; }
    }
}