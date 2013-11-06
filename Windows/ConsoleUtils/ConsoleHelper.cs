using System;
using System.Diagnostics;

namespace nucs.Windows.ConsoleUtils {
    public static class ConsoleHelper {
        public static void StartConsole() {
            NativeWin32.AllocConsole();
        }

        public static void StartConsole(string title) {
            NativeWin32.AllocConsole();
            Console.Title = title;
        }
    }
}