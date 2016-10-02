using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using nucs.Automation;
using nucs.Automation.Controllers;
using nucs.Automation.Mirror;
using nucs.Automation.Scripts;
using Win32Interop.Methods;
using Win32Interop.Structs;
using Condition = System.Windows.Automation.Condition;

namespace test {
    class Program {
        static void Main(string[] args) {
            //Starter.Run("chrome");
            Console.WriteLine("Started");
            Thread.Sleep(1000);

            var @o = Starter.Run("cmd", true, pr => pr.ProcessName == "cmd").Result;

            //Starter.Cmd("ping 127.0.0.1").Wait();
            Console.WriteLine("Done");
            Console.ReadLine();


            var p = SmartProcess.Get("notepad++");
            var win = p.MainWindow;
            win.BringToFront();
            //win.Mouse.MoveClick(300,300);
            win.Keyboard.Control(KeyCode.A);
            win.Keyboard.Down(KeyCode.Delete);
            Thread.Sleep(20);
            win.Keyboard.Up(KeyCode.Delete);
            Keyboard.Write("wow/waw\\amazing\n");

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static Keys ConvertCharToVirtualKey(char ch) {
            short vkey = VkKeyScan(ch);
            Keys retval = (Keys) (vkey & 0xff);
            int modifiers = vkey >> 8;
            if ((modifiers & 1) != 0)
                retval |= Keys.Shift;
            if ((modifiers & 2) != 0)
                retval |= Keys.Control;
            if ((modifiers & 4) != 0)
                retval |= Keys.Alt;
            return retval;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);
    }
}