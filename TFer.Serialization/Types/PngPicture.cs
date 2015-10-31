using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ProtoBuf;

namespace TFer.Commons.Objects {
    [ProtoContract]
    [Serializable]
    public class PngPicture {
        [ProtoMember(1)]
        [NonSerialized]
        public byte[] _data;
        
        public Bitmap Image;

        private PngPicture() { }

        public PngPicture(Bitmap image) {
            this.Image = image;
        }

        public PngPicture(Image img) {
            Image = new Bitmap(img);
        }

        [ProtoBeforeSerialization]
        private void BeforeSerialization() {
            using (var stream = new MemoryStream()) {
                Image.Save(stream, ImageFormat.Png);
                _data = stream.ToArray();
            }
        }

        [ProtoAfterDeserialization]
        private void AfterDeserialization() {
            using (Image image = System.Drawing.Image.FromStream(new MemoryStream(_data))) {
                Image = new Bitmap(image);
            }
            
        }
    }
}