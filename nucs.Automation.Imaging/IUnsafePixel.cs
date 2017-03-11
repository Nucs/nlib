using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
// ReSharper disable All

namespace nucs.Automation.Imaging {
    public unsafe interface IUnsafePixel {
        byte* position { get; set; }
        byte R { get; }
        byte G { get; }
        byte B { get; }
        byte A { get; }
    }

    public unsafe struct UnsafeAlphaPixel : IUnsafePixel {
        public byte* position { get; set; }
        public byte R => *(position + 2);
        public byte G => *(position + 1);
        public byte B => *(position + 0);
        public byte A => *(position + 3);

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public UnsafeAlphaPixel(byte* position) {
            this.position = position;
        }
    }

    public unsafe struct UnsafePixel : IUnsafePixel {
        static UnsafePixel() {
            fixed (byte* ptr = &FULLAPLHA) {
                FULLAPLHA_PTR = ptr;
            }
        }

        private static readonly byte FULLAPLHA =  255;
        private static readonly byte* FULLAPLHA_PTR;
        public byte* position { get; set; }
        public byte R => *(position + 2);
        public byte G => *(position + 1);
        public byte B => *(position + 0);
        public byte A => *(FULLAPLHA_PTR);

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public UnsafePixel(byte* position) {
            this.position = position;
        }
    }
}