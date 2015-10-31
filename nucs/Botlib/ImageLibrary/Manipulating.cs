using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using BotSuite.ImageLibrary;

namespace nucs.Botlib.ImageLibrary {
    public static class Manipulating {
        //Overload for crop that default starts top left of the image.
        public static ImageData CropImage(ImageData Image, int Height, int Width) { return CropImage(Image, 0, 0, Height, Width); }

        //The crop image sub
        public static ImageData CropImage(ImageData imageData,int StartAtX, int StartAtY, int Height, int Width) {
            var image = imageData.Bitmap;
            MemoryStream mm = null;
            try {
                //check the image height against our desired image height
                if (image.Height < Height) {
                    Height = image.Height;
                }

                if (image.Width < Width) {
                    Width = image.Width;
                }

                //create a bitmap window for cropping
                var bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                //create a new graphics object from our image and set properties
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //now do the crop
                grPhoto.DrawImage(image, new Rectangle(0, 0, Width, Height), StartAtX, StartAtY, Width, Height, GraphicsUnit.Pixel);

                // Save out to memory and get an image from it to send back out the method.
                mm = new MemoryStream();
                bmPhoto.Save(mm, ImageFormat.Jpeg);
                image.Dispose();
                bmPhoto.Dispose();
                grPhoto.Dispose();
                return new ImageData((Bitmap)Image.FromStream(mm));
            } catch (Exception ex) {
                throw new Exception("Error cropping image, the error was: " + ex.Message);
            }
        }

        //todo implement
        /*//Hard resize attempts to resize as close as it can to the desired size and then crops the excess
        public static Image HardResizeImage(int Width, int Height, Image Image) {
            int width = Image.Width;
            int height = Image.Height;
            Image resized = null;
            if (Width > Height) {
                resized = ResizeImage(Width, Width, Image);
            } else {
                resized = ResizeImage(Height, Height, Image);
            }
            Image output = CropImage(resized, Height, Width);
            //return the original resized image
            return output;
        }*/

        //Image resizing
        public static Image ResizeImage(int maxWidth, int maxHeight, Image Image) {
            int width = Image.Width;
            int height = Image.Height;
            if (width > maxWidth || height > maxHeight) {
                //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                //from displaying as the thumbnail image later, in other words, we want a clean
                //resize, not a grainy one.
                Image.RotateFlip(RotateFlipType.Rotate180FlipX);
                Image.RotateFlip(RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                if (width > height) {
                    ratio = width/(float) height;
                    width = maxWidth;
                    height = Convert.ToInt32(Math.Round(width/ratio));
                } else {
                    ratio = height/(float) width;
                    height = maxHeight;
                    width = Convert.ToInt32(Math.Round(height/ratio));
                }

                //return the resized image
                return Image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
            //return the original resized image
            return Image;
        }
    }
}