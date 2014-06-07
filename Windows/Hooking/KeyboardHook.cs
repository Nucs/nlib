/*/*using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseKeyboardLibrary {
    /// <summary>
    /// Captures global keyboard events
    /// </summary>
    public class KeyboardHook : GlobalHook {
        #region Events

        public delegate void KeyPressedEventHandler(object sender, KeyPressedEventArgs args);
        public event KeyPressedEventHandler KeyDown;
        public event KeyPressedEventHandler KeyUp;
        public event KeyPressedEventHandler KeyPress;

        #endregion

        #region Constructor

        public KeyboardHook() {
            _hookType = WH_KEYBOARD_LL;
        }

        #endregion

        #region Methods

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam) {
            bool handled = false;

            if (nCode > -1 && (KeyDown != null || KeyUp != null || KeyPress != null)) {
                KeyboardHookStruct keyboardHookStruct =
                    (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (KeyboardHookStruct));

                // Is Control being held down?
                bool control = ((GetKeyState(VK_LCONTROL) & 0x80) != 0) ||
                               ((GetKeyState(VK_RCONTROL) & 0x80) != 0);

                // Is Shift being held down?
                bool shift = ((GetKeyState(VK_LSHIFT) & 0x80) != 0) ||
                             ((GetKeyState(VK_RSHIFT) & 0x80) != 0);

                // Is Alt being held down?
                bool alt = ((GetKeyState(VK_LALT) & 0x80) != 0) ||
                           ((GetKeyState(VK_RALT) & 0x80) != 0);

                // Is Win being held down?
                bool win = ((GetKeyState(VK_LWIN) & 0x80) != 0) ||
                           ((GetKeyState(VK_RWIN) & 0x80) != 0);

                // Is CapsLock on?
                bool capslock = (GetKeyState(VK_CAPITAL) != 0);

                // Create event using keycode and control/shift/alt values found above
                var keys = (Keys) (
                               keyboardHookStruct.vkCode |
                               (control ? (int) Keys.Control : 0) |
                               (shift ? (int) Keys.Shift : 0) |
                               (alt ? (int) Keys.Alt : 0) |
                               (win ? (int) Keys.LWin : 0)
                           );

                var e = new KeyPressedEventArgs(keys, control, alt, shift, win, capslock);

                // Handle KeyDown and KeyUp events
                switch (wParam) {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        if (KeyDown != null) {
                            KeyDown(this, e);
                            handled = handled || e.Handled;
                        }
                        break;
                    case WM_KEYUP:
                    case WM_SYSKEYUP:
                        if (KeyUp != null) {
                            KeyUp(this, e);
                            handled = handled || e.Handled;
                        }
                        break;
                }

                // Handle KeyPress event
                if (wParam == WM_KEYDOWN &&
                    !handled &&
                    !e.SuppressKeyPress &&
                    KeyPress != null) {
                    byte[] keyState = new byte[256];
                    byte[] inBuffer = new byte[2];
                    GetKeyboardState(keyState);

                    if (ToAscii(keyboardHookStruct.vkCode,
                                keyboardHookStruct.scanCode,
                                keyState,
                                inBuffer,
                                keyboardHookStruct.flags) == 1) {
                        char key = (char) inBuffer[0];
                        if ((capslock ^ shift) && Char.IsLetter(key))
                            key = Char.ToUpper(key);
                        KeyPressEventArgs e2 = new KeyPressEventArgs(key);
                        KeyPress(this, e2);
                        handled = handled || e.Handled;
                    }
                }
            }

            if (handled)
                return 1;
            else
                return CallNextHookEx(_handleToHook, nCode, wParam, lParam);
        }

        #endregion
    }

    public class KeyPressedEventArgs {
        internal KeyPressedEventArgs(Keys keyPressed, bool control, bool alt, bool shift, bool windows, bool capslock) {}

        public Keys KeyPressed { get; private set; }
        public bool ControlPressed { get; private set; }
        public bool AltPressed { get; private set; }
        public bool ShiftPressed { get; private set; }
        public bool WindowPressed { get; private set; }
        public bool CapsLockPressed { get; private set; }

        public bool Handled { get; set; }
    }
}#1#

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nucs.Utils;

namespace nucs.Windows.Hooking {
    public delegate void KeyPressedEventHandler(object sender, KeyPressEventArgs e);

    /// <summary>
    /// Captures global keyboard events
    /// </summary>
    public class KeyboardHook : AbstractGlobalHook {
        #region Events

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event KeyPressedEventHandler KeyPress;

        #endregion

        #region Constructor

        public KeyboardHook() {
            _hookType = WH_KEYBOARD_LL;
        }

        #endregion

        #region Methods

        private readonly CountingInvoker counter = new CountingInvoker(10, true, GC.Collect);
        private readonly object lock_event_invoker = new object();

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam) {
            bool handled = false;

            if (nCode > -1 && (KeyDown != null || KeyUp != null || KeyPress != null)) {
                var keyboardHookStruct =
                    (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (KeyboardHookStruct));

                // Is Control being held down?
                bool control = ((GetKeyState(VK_LCONTROL) & 0x80) != 0) ||
                               ((GetKeyState(VK_RCONTROL) & 0x80) != 0);

                // Is Shift being held down?
                bool shift = ((GetKeyState(VK_LSHIFT) & 0x80) != 0) ||
                             ((GetKeyState(VK_RSHIFT) & 0x80) != 0);

                // Is Alt being held down?
                bool alt = ((GetKeyState(VK_LALT) & 0x80) != 0) ||
                           ((GetKeyState(VK_RALT) & 0x80) != 0);

                // Is CapsLock on?
                bool capslock = (GetKeyState(VK_CAPITAL) != 0);

                // Create event using keycode and control/shift/alt values found above
                var e = new KeyEventArgs(
                    (Keys) (
                               keyboardHookStruct.vkCode |
                               (control ? (int) Keys.Control : 0) |
                               (shift ? (int) Keys.Shift : 0) |
                               (alt ? (int) Keys.Alt : 0)
                           ));

                //lock to make it ordered and not chaotic.
                lock (lock_event_invoker) {
                    // Handle KeyDown and KeyUp events
                    switch (wParam) {
                            #region Key Down

                        case WM_KEYDOWN:
                            goto _keydown;
                        case WM_SYSKEYDOWN:
                            _keydown:
                            if (KeyDown != null) {
                                KeyDown(this, e);
                                handled = handled || e.Handled;
                            }
                            break;

                            #endregion

                            #region Key Up

                        case WM_KEYUP:
                            goto _keyup;
                        case WM_SYSKEYUP:
                            _keyup:
                            if (KeyUp != null) {
                                KeyUp(this, e);
                                handled = handled || e.Handled;
                            }
                            break;

                            #endregion
                    }

                    // Handle KeyPress event
                    if (wParam == WM_KEYDOWN && !handled && !e.SuppressKeyPress && KeyPress != null) {
                        #region Key Press

                        var keyState = new byte[256];
                        var inBuffer = new byte[2];
                        GetKeyboardState(keyState);

                        if (
                            ToAscii(keyboardHookStruct.vkCode, keyboardHookStruct.scanCode, keyState, inBuffer,
                                    keyboardHookStruct.flags) == 1) {
                            var key = (char) inBuffer[0];
                            if ((capslock ^ shift) && Char.IsLetter(key))
                                key = Char.ToUpper(key);
                            var e2 = new KeyPressEventArgs((Keys) (
                               keyboardHookStruct.vkCode |
                               (control ? (int) Keys.Control : 0) |
                               (shift ? (int) Keys.Shift : 0) |
                               (alt ? (int) Keys.Alt : 0)
                           ));
                            KeyPress(this, e2);
                            handled = handled || e.Handled;
                        }

                        #endregion
                    }
                    counter.Up();
                }
            }
            return handled ? 1 : CallNextHookEx(_handleToHook, nCode, wParam, lParam);
        }

        #endregion
    }

    public class KeyPressEventArgs : EventArgs {
        private readonly Keys keyData;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Forms.KeyEventArgs"/> class.
        /// </summary>
        /// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys"/> representing the key that was pressed, combined with any modifier flags that indicate which CTRL, SHIFT, and ALT keys were pressed at the same time. Possible values are obtained be applying the bitwise OR (|) operator to constants from the <see cref="T:System.Windows.Forms.Keys"/> enumeration. </param>
        public KeyPressEventArgs(Keys keyData) {
            this.keyData = keyData;
        }

        /// <summary>
        /// Gets a value indicating whether the ALT key was pressed.
        /// </summary>
        /// 
        /// <returns>
        /// true if the ALT key was pressed; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public virtual bool Alt {
            get { return (keyData & Keys.Alt) == Keys.Alt; }
        }

        /// <summary>
        /// Gets a value indicating whether the CTRL key was pressed.
        /// </summary>
        /// 
        /// <returns>
        /// true if the CTRL key was pressed; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public bool Control {
            get { return (keyData & Keys.Control) == Keys.Control; }
        }

        /// <summary>
        /// Gets the keyboard code for a <see cref="E:System.Windows.Forms.Control.KeyDown"/> or <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Keys"/> value that is the key code for the event.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public Keys KeyCode {
            get {
                Keys keys = keyData & Keys.KeyCode;
                if (!Enum.IsDefined(typeof (Keys), keys))
                    return Keys.None;
                else
                    return keys;
            }
        }

        /// <summary>
        /// Gets the keyboard value for a <see cref="E:System.Windows.Forms.Control.KeyDown"/> or <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// 
        /// <returns>
        /// The integer representation of the <see cref="P:System.Windows.Forms.KeyEventArgs.KeyCode"/> property.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public int KeyValue {
            get { return (int) (keyData & Keys.KeyCode); }
        }

        /// <summary>
        /// Gets the key data for a <see cref="E:System.Windows.Forms.Control.KeyDown"/> or <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Keys"/> representing the key code for the key that was pressed, combined with modifier flags that indicate which combination of CTRL, SHIFT, and ALT keys was pressed at the same time.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public Keys KeyData {
            get { return keyData; }
        }

        /// <summary>
        /// Gets the modifier flags for a <see cref="E:System.Windows.Forms.Control.KeyDown"/> or <see cref="E:System.Windows.Forms.Control.KeyUp"/> event. The flags indicate which combination of CTRL, SHIFT, and ALT keys was pressed.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Keys"/> value representing one or more modifier flags.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public Keys Modifiers {
            get { return keyData & Keys.Modifiers; }
        }

        /// <summary>
        /// Gets a value indicating whether the SHIFT key was pressed.
        /// </summary>
        /// 
        /// <returns>
        /// true if the SHIFT key was pressed; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public virtual bool Shift {
            get { return (keyData & Keys.Shift) == Keys.Shift; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the key event should be passed on to the underlying control.
        /// </summary>
        /// 
        /// <returns>
        /// true if the key event should not be sent to the control; otherwise, false.
        /// </returns>
    }
}*/