using System;
using System.Windows.Media.Imaging;

namespace nucs.Toaster.Resources {
    public static class Images {
        public static ToastImage Welcome;
        public static ToastImage SuccessThumbs;
        public static ToastImage BrokenThumbs;
        public static ToastImage Finished;
        public static ToastImage Info;
        public static ToastImage Mobile;
        
        public static void Load() {
            Welcome = new ToastImage(new BitmapImage(new Uri("/nucs.Toaster;component/Resources/Welcome.png", UriKind.RelativeOrAbsolute)));
            SuccessThumbs = new ToastImage(new BitmapImage(new Uri("/nucs.Toaster;component/Resources/Downloading.png", UriKind.RelativeOrAbsolute)));
            BrokenThumbs = new ToastImage(new BitmapImage(new Uri("/nucs.Toaster;component/Resources/Failed.png", UriKind.RelativeOrAbsolute)));
            Finished = new ToastImage(new BitmapImage(new Uri("/nucs.Toaster;component/Resources/Finished.png", UriKind.RelativeOrAbsolute)));
            Info = new ToastImage(new BitmapImage(new Uri("/nucs.Toaster;component/Resources/Info.png", UriKind.RelativeOrAbsolute)));
            Mobile = new ToastImage(new BitmapImage(new Uri("/nucs.Toaster;component/Resources/Mobile.png", UriKind.RelativeOrAbsolute)));
        }

    }

    public class ToastImage {
        public static ToastImage Welcome => Images.Welcome;
        public static ToastImage SuccessThumbs => Images.SuccessThumbs;
        public static ToastImage BrokenThumbs => Images.BrokenThumbs;
        public static ToastImage Finished => Images.Finished;
        public static ToastImage Info => Images.Info;
        public static ToastImage Mobile => Images.Mobile;

        public ToastImage(BitmapImage image) {
            Image = image;
        }

        public BitmapImage Image { get; }

        public static implicit operator BitmapImage(ToastImage t) {
            return t.Image;
        }
    }
}