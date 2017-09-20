using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using nucs.Automation.Mirror;

namespace nucs.Automation.Controllers {
    public class WindowSpecificKeyboardController : IExtendedKeyboardController {
        /// <summary>
        ///     The delay between regular actions - e.g. when pressing: down, commondelay, up.
        /// </summary>
        public int CommonDelay { get; set; }

        /// <summary>
        ///     The targeted process
        /// </summary>
        public SmartProcess Process { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public WindowSpecificKeyboardController(SmartProcess process) {
            if (process == null) throw new ArgumentNullException(nameof(process));
            if (process.HasExited) throw new Exception("The given SmartProcess has exited!");
            Process = process;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public WindowSpecificKeyboardController(Process process) : this(SmartProcess.Get(process)) {}

        /*private void _ensureExisting() {
            if (Process.HasExited) throw new InvalidOperationException("The given process has exited already.");
        }*/

        #region Implementation of IKeyboardController

        /// <summary>
        ///     Writes down the char that this key represents as if it was through the keyboard. - won't work on Keys like 'End' or 'Backspace' or 'Control'
        /// </summary>
        public void Write(Keys key) {
            this.Write((char)key);
        }

        /// <summary>
        ///     Writes down this string as if it was through the keyboard.
        /// </summary>
        public void Write(string text) {
            this.Write(text.ToCharArray());
        }

        /// <summary>
        ///     Writes down the char that this key represents as if it was through the keyboard. - won't work on Keys like 'End' or 'Backspace' or 'Control'
        /// </summary>
        public void Write(KeyCode keycode) {
            this.Write((char)Native.MapVirtualKey((uint)keycode, 2));
        }

        /// <summary>
        ///     Writes down the char that this key represents as if it was through the keyboard.
        /// </summary>
        public void Write(char @char) {
            this.Write(new[] { @char });
        }

        /// <summary>
        ///     Writes down the characters as if it was through the keyboard.
        /// </summary>
        public void Write(params char[] chars) {
            foreach (var c in chars) {
                    Native.SendMessage(Process.MainWindowHandle, WM.CHAR, (int)c, 0);
            }
        }

        /// <summary>
        ///     Writes down the characters as if it was through the keyboard.
        /// </summary>
        public void Write(int utf32) {
            //copied from KeyboardController
            string unicodeString = Char.ConvertFromUtf32(utf32);
            this.Write(unicodeString);
        }

        /// <summary>
        ///     Presses down this keycode.
        /// </summary>
        public void Down(KeyCode keycode) {
            Native.SendMessage(Process.MainWindowHandle, WM.KEYDOWN, (int) keycode, 0);
        }

        /// <summary>
        ///     Releases this keycode.
        /// </summary>
        public void Up(KeyCode keycode) {
            Native.SendMessage(Process.MainWindowHandle, WM.KEYUP, (int) keycode, 0);
        }

        /// <summary>
        ///     Presses down and releases this keycode with the given delay between them
        /// </summary>
        /// <param name="keycode">The keycode to press</param>
        /// <param name="delay">The delay between the actions in milliseconds</param>
        public void Press(KeyCode keycode, uint delay = 20) {
            Down(keycode);
            Thread.Sleep((int) delay);
            Up(keycode);
        }

        #endregion

        #region Implementation of IExtendedKeyboardController

        public void Enter() {
            throw new NotImplementedException();
        }

        public void Back() {
            throw new NotImplementedException();
        }

        public void Alt(KeyCode keycode) {
            throw new NotImplementedException();
        }

        public void Shift(KeyCode keycode) {
            throw new NotImplementedException();
        }

        public void Control(KeyCode keycode) {
            throw new NotImplementedException();
        }

        public void Win(KeyCode keycode) {
            throw new NotImplementedException();
        }

        public void PressAsync(KeyCode keycode, uint delay = 20) {
            throw new NotImplementedException();
        }

        #endregion
    }
}