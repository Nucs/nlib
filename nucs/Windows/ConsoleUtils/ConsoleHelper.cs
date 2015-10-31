using System;
using System.Diagnostics;

namespace nucs.Windows.ConsoleUtils {
    public static class ConsoleHelper {
        public static void StartConsole() {
            NativeWin32.FreeConsole();
            NativeWin32.AllocConsole();
        }

        public static void StartConsole(string title) {
            StartConsole();
            Console.Title = title;
        }
    }
}