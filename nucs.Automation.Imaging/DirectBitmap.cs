using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using nucs.Automation.Imaging.Helpers;

namespace nucs.Automation.Imaging {
    public partial class DirectBitmap : IDisposable {
        /// <summary>
        ///     A map of where pixels are not transparent
        /// </summary>
        public AlphaMap Map { get; protected set; }
        /// <summary>
        ///     A bitmap that represents the bytes in <see cref="Bits"/> parameter.
        /// </summary>
        public Bitmap Bitmap { get; private set; }
        /// <summary>
        ///     The data of the bitmap/image.
        /// </summary>
        public byte[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        /// <summary>
        ///     Height of the bitmap
        /// </summary>
        public int Height { get; }
        /// <summary>
        ///     Width of the bitmap
        /// </summary>
        public int Width {get;}
        /// <summary>
        ///     How many pixels are there in this image.
        /// </summary>
        public int Pixels => Width * Height;
        /// <summary>
        ///     The amount of bytes it takes in a single row of pixels in current image.
        /// </summary>
        public int Stride => Width * PixelSize;
        /// <summary>
        ///     The absolute location in memory this image is storage starts.
        /// </summary>
        public IntPtr Scan0 { get; private set; }
        /// <summary>
        ///     How many bytes a single pixel is
        /// </summary>
        public int PixelSize => (Format == PixelFormat.Format32bppArgb ? 4 : 3);
        /// <summary>
        ///     The format of the image
        /// </summary>
        public PixelFormat Format => Bitmap.PixelFormat;
        /// <summary>
        ///     The handle for Garbage Collector
        /// </summary>
        protected GCHandle BitsHandle { get; private set; }

        /// <summary>
        ///     The 'rectangle' of this image
        /// </summary>
        public Rectangle Area => new Rectangle(0, 0, Width, Height);

        public DirectBitmap(int width, int height, PixelFormat format) {
            switch (format) {
                case PixelFormat.Format24bppRgb:
                    Bits = new byte[width * height * 3];
                    BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
                    Bitmap = new Bitmap(width, height, width * 3, format, BitsHandle.AddrOfPinnedObject());
                    break;
                case PixelFormat.Format32bppArgb:
                    Bits = new byte[width * height * 4];
                    BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
                    Bitmap = new Bitmap(width, height, width * 4, format, BitsHandle.AddrOfPinnedObject());
                    break;
                default:
                    throw new InvalidOperationException(nameof(format));
            }

            Scan0 = Marshal.UnsafeAddrOfPinnedArrayElement(Bits, 0);
            unsafe {
                _scan0 = (byte*) Scan0.ToPointer();
            }
            Height = height;
            Width = width;
        }

        public DirectBitmap(Image img) : this(new Bitmap(img)) {}
        protected DirectBitmap() { }
        public DirectBitmap(Bitmap bmp) {
            Bitmap _disposelater = null;
            if (bmp.PixelFormat == PixelFormat.Format32bppRgb) {
                var newBmp = new Bitmap(bmp);
                bmp = _disposelater = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), PixelFormat.Format32bppArgb);
                newBmp.Dispose();
            }

            if (bmp.PixelFormat != PixelFormat.Format24bppRgb && bmp.PixelFormat != PixelFormat.Format32bppArgb)
                throw new InvalidOperationException(nameof(bmp.PixelFormat));
            var pixelformat = bmp.PixelFormat == PixelFormat.Format32bppRgb ? PixelFormat.Format32bppArgb : bmp.PixelFormat;
            var bdata = bmp.LockBits(new Rectangle(0, 0, Width=bmp.Width, Height=bmp.Height), ImageLockMode.ReadWrite, pixelformat);
            try {
                Bits = new byte[bdata.Stride * bdata.Height];
                Copying.MemCopy(bdata.Scan0, Marshal.UnsafeAddrOfPinnedArrayElement(Bits, 0), bdata.Stride * bdata.Height);
            } finally {
                bmp.UnlockBits(bdata);
            }

            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            switch (pixelformat) {
                case PixelFormat.Format24bppRgb:
                    Bitmap = new Bitmap(bmp.Width, bmp.Height, bmp.Width * 3, pixelformat, BitsHandle.AddrOfPinnedObject());
                    break;
                case PixelFormat.Format32bppArgb:
                    Bitmap = new Bitmap(bmp.Width, bmp.Height, bmp.Width * 4, pixelformat, BitsHandle.AddrOfPinnedObject());
                    break;
                default:
                    throw new InvalidOperationException(nameof(bmp.PixelFormat));
            }

            Scan0 = Marshal.UnsafeAddrOfPinnedArrayElement(Bits, 0);
            unsafe {
                _scan0 = (byte*) Scan0.ToPointer();
            }
            _disposelater?.Dispose();
        }

        /// <summary>
        ///     Initialize a DirectBitmap from embedded resource, part of the name will be enough to find it.
        /// </summary>
        /// <param name="embeddedresource_name"></param>
        public DirectBitmap(string embeddedresource_name) : this(Image.FromStream(GetResource(embeddedresource_name))) {}

        protected readonly unsafe byte* _scan0;
        public unsafe byte* this[int position] => _scan0 + position * 3;

        public unsafe byte* this[int x, int y] => _scan0 + ((x + y * Width) * PixelSize);

        public unsafe UnsafePixel GetPixel(int linearposition) {
            return new UnsafePixel(this[linearposition]);
        }

        public unsafe UnsafePixel GetPixel(int x, int y) {
            return new UnsafePixel(this[x, y]);
        }

        public void Dispose() {
            if (Disposed)
                return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }

        private static Stream GetResource(string name) {
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asam => asam.GetManifestResourceNames().Any(mrn => mrn.Contains(name)));
            var target = asm.GetManifestResourceNames().FirstOrDefault(mrn => mrn.Contains(name));
            if (target == null)
                throw new FileNotFoundException($"Could not find a resource that contains the name '{name}'");
            return asm.GetManifestResourceStream(target);
        }
    }
}