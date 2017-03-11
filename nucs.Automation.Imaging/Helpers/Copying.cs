using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace nucs.Automation.Imaging.Helpers {
    internal static class Copying {
        public static unsafe void MemCopy(IntPtr pSource, IntPtr pDest, int Len) {
            unchecked {
                int count = Len / Marshal.SizeOf(typeof(int));
                int rest = Len % count;
                int* ps = (int*) pSource.ToPointer(), pd = (int*) pDest.ToPointer();
                // Loop over the cnt in blocks of 4 bytes, copying an integer (4 bytes) at a time:
                for (int n = 0; n < count; n++)
                    *pd++ = *ps++;
                // Complete the copy by moving any bytes that weren't moved in blocks of 4:
                if (rest > 0) {
                    byte* ps1 = (byte*) ps;
                    byte* pd1 = (byte*) pd;
                    for (int n = 0; n < rest; n++)
                        *pd1++ = *ps1++;
                }
                //Console.WriteLine(ByteArrayCompare((byte*) pSource.ToPointer(), (byte*) pDest.ToPointer(), Len));
            }
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe int memcmp(byte* b1, byte* b2, long count);

        public static unsafe bool MemCompare(byte* b1, byte* b2, int len)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return memcmp(b1, b2, len) == 0;
        }
        public static unsafe void MemCopy(byte* source, byte* dest, int size) {
            MemCopy(new IntPtr(source), new IntPtr(dest), size);
        }

        public static unsafe void MemCopy(int* source, int* dest, int size) {
            MemCopy(new IntPtr(source), new IntPtr(dest), size* Marshal.SizeOf(typeof(int)));
        }

    }
}