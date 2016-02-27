using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace nucs.Windows.ConsoleUtils {
    public static class ConsoleHelper {
        [DllImport("kernel32.dll")]
        private static extern int FreeConsole();
        [DllImport("kernel32")]
        private static extern bool AllocConsole();
        public static void StartConsole() {
            FreeConsole();
            AllocConsole();
        }

        public static void StartConsole(string title) {
            StartConsole();
            try { 
                Console.Title = title;
            } catch (IOException) { }
        }
    }
}