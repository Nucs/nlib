using System;
using System.Linq;

namespace nucs.SystemCore.String {
    public static class StringToOthers {
        #region Numbers
        public static long ToInt64(this string toConvert) {
            try { return Convert.ToInt64(toConvert); } catch { return 0; }
        }
        public static int ToInt32(this string toConvert) {
            try { return Convert.ToInt32(toConvert); } catch { return 0; }
        }
        public static short ToInt16(this string toConvert) {
            try { return Convert.ToInt16(toConvert); } catch { return 0; }
        }

        public static ulong ToUInt64(this string toConvert) {
            try { return Convert.ToUInt64(toConvert); } catch { return 0; }
        }

        public static uint ToUInt32(this string toConvert) {
            try { return Convert.ToUInt32(toConvert); } catch { return 0; }
        }

        public static ushort ToUInt16(this string toConvert) {
            try { return Convert.ToUInt16(toConvert); } catch { return 0; }
        }

        public static decimal ToDecimal(this string toConvert) {
            try { return Convert.ToDecimal(toConvert); } catch { return 0; }
        }

        public static double ToDouble(this string toConvert) {
            try { return Convert.ToDouble(toConvert); } catch { return 0; }
        }

        public static float ToFloat(this string toConvert) {
            try { return Convert.ToSingle(toConvert); } catch { return 0; }
        }

        #endregion

        public static char[] ToChars(this string toConvert) {
            try { return toConvert.ToCharArray(); } catch { return null; }
        }

        public static string ConvertToString(this char[] chars) {
            return new string(chars);
        }

    }
}
