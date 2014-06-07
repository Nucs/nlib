﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using nucs.Forms;

namespace nucs.Windows.Keyboard {
    /// <summary>
    /// Provides a hotkey binding tool.
    /// </summary>
    public class KeyboardListener : BaseKeyboardManager {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private readonly bool IsConsole;
        public KeyboardListener(bool IsConsoleApplication = false) {
            IsConsole = IsConsoleApplication;
        }


        #region Events

        /// <summary>
        /// Disablemultiple (basically a spam) of firing button down event. True by default.
        /// </summary>
        public bool SuppressKeyHold {
            get { return _suppressKeyHold; }
            set { _suppressKeyHold = value; }
        }
        private bool _suppressKeyHold = true;

        #endregion

        /// <summary>
        ///     Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        private IntPtr HHook = IntPtr.Zero;

        protected keyboardHookProc callbackDelegate;

        public override bool IsHooked { get; protected set; }



        protected ApplicationSimulator AppSim = null;
        protected override bool Hook() {
            if (IsHooked) return false;
            if (IsConsole) {
                AppSim = ApplicationSimulator.Create();
                AppSim.Invoke(()=>hook(
                    AppSim.InvokeReturn(() => 
                        NativeWin32.LoadLibrary("user32.dll"))
                    ));
            } else 
                hook(NativeWin32.LoadLibrary("user32.dll"));
            return true;
        }

        protected override bool Unhook() {
            if (!IsHooked) return false;
            if (IsConsole) {
                AppSim.Invoke(() => { unhook(); AppSim.Close(); });
                AppSim = null;
            } else
                unhook();
            return true;
        }

        private void hook(IntPtr hInstance) {
            callbackDelegate = new keyboardHookProc(hookProc);
            GC.KeepAlive(callbackDelegate);
            HHook = NativeWin32.SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, hInstance, 0);
            if (HHook == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        ///     Uninstalls the global hook
        /// </summary>
        private void unhook() {
            if (NativeWin32.UnhookWindowsHookEx(HHook) == false) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private bool rctrl, ralt, rshift;
        private bool lctrl, lalt, lshift;
        private Keys tmp_key;
        private ModKeys tmp_mods;
        private bool was_down = false;
        private bool[] duplicatetags = new bool[256];
        private bool[] presstags= new bool[256];

        /// <summary>
        ///     The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        internal int hookProc(int code, int wParam, ref keyboardHookStruct lParam) {
            var keyc = ((KeyCode)lParam.vkCode);
            var key = keyc.ToKeys();
            var modifier = ModKeys.None;
            if (code >= 0 && (!SuppressKeyHold || duplicatetags[(int)keyc] != (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))) {
                #region Modifier Switches
                if (keyc.IsModifier()) {
                    switch (keyc) {
                        case KeyCode.RControl:
                            rctrl = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN; //is key down? otherwise false.
                            break;
                        case KeyCode.LControl:
                            lctrl = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN; //is key down? otherwise false.
                            break;
                        case KeyCode.RAlt:
                            ralt = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN; //is key down? otherwise false.
                            break;
                        case KeyCode.LAlt:
                            lalt = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN; //is key down? otherwise false.
                            break;
                        case KeyCode.RShift:
                            rshift = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN; //is key down? otherwise false.
                            break;
                        case KeyCode.LShift:
                            lshift = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN; //is key down? otherwise false.
                            break;
                    }
                } else {
                    if (rctrl) modifier |= ModKeys.RControl;
                    if (lctrl) modifier |= ModKeys.LControl;
                    if (rshift) modifier |= ModKeys.RControl;
                    if (lshift) modifier |= ModKeys.LShift;
                    if (ralt) modifier |= ModKeys.RAlt;
                    if (lalt) modifier |= ModKeys.LAlt;
                }


                #endregion
                #region Args and Event firing

                //Console.WriteLine("(" + ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) ? "D" : "U") + ") " + modifier.ToString() + " + " + key.ToString());
                
                Hotkey hotk;
                HotkeyEventArgs args = null;
                if ((hotk = base._registers.FirstOrDefault(hk => hk.Key.Equals(key) && hk.Modifiers.Compare(modifier))) != null) {
                    //keydown
                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)) {
                        args = InvokeKeyDown(this, hotk);
                    } else { //key up
                        if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP)) {
                            args = InvokeKeyUp(this, hotk);

                            //if equals to last key event is up and equals to this key event -> KeyPress
                            // hotkey mode: /*tmp_key.CompareModifiers(key) && tmp_mods.CompareModifiers(modifier) && !(wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)*/
                            if (args == null || (args.Handled == false && presstags[(int)keyc])) {
                                args = InvokeKeyPress(this, hotk);
                            }
                        }
                    }

                }

                if (args != null && args.Handled)
                        return 1;

                #endregion


                presstags[(int) keyc] = wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN;
                if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) {
                    tmp_key = key;
                    tmp_mods = modifier;
                } else {
                    tmp_key = Keys.None;
                    tmp_mods = ModKeys.None;
                }
            }
            if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && duplicatetags[(int)keyc] == false)
                duplicatetags[(int)keyc] = true;
            else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && duplicatetags[(int)keyc] == true)
                duplicatetags[(int)keyc] = false;

            
            return NativeWin32.CallNextHookEx(HHook, code, wParam, ref lParam);
        }
    }
}