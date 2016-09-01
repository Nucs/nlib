using System;
using System.Drawing;
using System.Linq;
using Colorful;
using Console = Colorful.Console;

namespace nucs.SConsole {
    public static class Consoler {
        public static bool AskQuestion(string question, bool @default) {
            _reprint:
            Console.Write("[DECIDE] ", Color.DarkOrange);
            Console.Write(question, Color.White);
            Console.Write($"{(question.Last()==' ' ? "" :" ")}(default:[{(@default?"y":"n")}] - [y/n]) >", Color.Aqua);

            var r = Console.ReadLine().ToLowerInvariant();
            if (r == "y" || r == "yes" || r == "true")
                return true;
            if (r == "n" || r == "no" || r == "hellno" || r == "nope" || r == "false")
                return false;
            if (string.IsNullOrEmpty(r))
                return @default;
            Error("Invalid Input");
            goto _reprint;
        }

        /// <summary>
        ///     Prints an exception
        /// </summary>
        public static void Error(string msg) {
            Log("ERROR", Color.Red, msg);
        }

        /// <summary>
        ///     Prints an exception
        /// </summary>
        public static void Error(Exception e) {
#if DEBUG
            Error(e.ToString());
#else
            Error(e.Message);
#endif
        }

        /// <summary>
        /// Logs with color and type for example: [ERROR] message
        /// </summary>
        /// <param name="type">for [ERROR] type ERROR</param>
        /// <param name="color">Color is for the [ERROR]</param>
        /// <param name="message">The message</param>
        public static void Log(string type, Color color, string message) {
            Console.Write($"[{type}] ", color);
            Console.WriteLine(message, Color.White);
        }
    }
}