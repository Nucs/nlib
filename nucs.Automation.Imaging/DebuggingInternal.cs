using System;
using System.Diagnostics;

namespace nucs.Summoners {
    internal static class Debugging {
        public static Stopwatch sw = new Stopwatch();

        public static void Print(string s = null) {
            Console.WriteLine($"{s} : {sw.ElapsedMilliseconds}");
        }

        public static void Restart() {
            sw.Restart();
        }
    }
}