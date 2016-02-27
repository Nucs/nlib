using System;
using System.Collections.Generic;
namespace nucs.SystemCore.String {
    public static class StringAsParameterPassing {
        public static readonly char ShortSplitLetter = '¶';
        public static readonly string ShortSplitString = "¶";

        public static IList<string> AsStrings(this string s) {
            return s.Split(ShortSplitLetter);
        }

        public static Dictionary<string, string> ProcessParameters(this string combined) {
            return ProcessParameters(combined.AsStrings());
        }

        public static Dictionary<string, string> ProcessParameters(this IEnumerable<string> strings) {
            var dic = new Dictionary<string, string>();
            foreach (var s in strings) {
                try {
                    var spl = s.Split(':');
                    dic.Add(spl[0], spl[1]);
                } catch (Exception) {}
            }
            return dic;
        }
    
    }

    
}
