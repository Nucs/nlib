using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using nucs.Collections;

namespace nucs.Windows.Keyboard {

    public delegate void KeyEventHandler(BaseKeyboardManager sender, HotkeyEventArgs args);
    public abstract class BaseKeyboardManager : IDisposable {

        protected readonly ImprovedList<Hotkey> _registers = new ImprovedList<Hotkey>();

        public ReadOnlyCollection<Hotkey> Registers {
            get { return _registers.AsReadOnly(); }
        }

        /// <summary>
        ///     Occurs when one of the hooked keys is pressed down and then released
        /// </summary>
        public event KeyEventHandler KeyPress;

        /// <summary>
        ///     Occurs when one of the hooked keys is pressed down
        /// </summary>
        public event KeyEventHandler KeyUp;
        /// <summary>
        ///     Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyDown;
        public abstract bool IsHooked { get; protected set; }

        /// <summary>
        /// Hooks or begins the listen
        /// </summary>
        protected abstract bool Hook();

        /// <summary>
        /// Stops the Hook and releases unmanaged resources.
        /// </summary>
        protected abstract bool Unhook();


        /// <summary>
        /// Clears the registeration without unhooking.
        /// </summary>
        public virtual void Clear() {
            _registers.Clear();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        /// <param name="description"></param>
        /// <returns>If added successfully, returns the <see cref="Hotkey"/> that was created, otherwise null.</returns>
        public virtual Hotkey Register(Keys key, ModKeys modifiers, string description) {
            if (_registers.Any(hkk => string.Equals(hkk.Description, description) || (hkk.Modifiers == modifiers && hkk.Key == key)))
                return null;
            Hotkey hk = Hotkey.Create(key, modifiers, description);
            _registers.Add(hk);
            if (IsHooked == false)
                try {
                    IsHooked = Hook();
                } catch (Exception e) {
                    throw new Exception("Failed to perform hook. see inner exception.", e);
                }
           return hk;
        }

        public virtual Hotkey Register(Keys key, string description) { return Register(key, ModKeys.None, description); }

        public int Unregister(Keys key) {
            return Unregister(key, ModKeys.None);
        }

        public virtual int Unregister(Keys key, ModKeys modifier) {
            var count = _registers.RemoveWhere(hk => hk.Key == key && hk.Modifiers == modifier);
            if (_registers.Count == 0 && IsHooked)
                try {
                    IsHooked = Unhook();
                } catch (Exception e) {
                    throw new Exception("Failed to perform unhook. see inner exception.", e);
                }
            return count;
        }

        public virtual int Unregister(string description) {
            var count = _registers.RemoveWhere(hk => hk.Description.Equals(description));
            if (_registers.Count == 0 && IsHooked)
                try {
                    IsHooked = Unhook();
                } catch (Exception e) {
                    throw new Exception("Failed to perform unhook. see inner exception.", e);
                }
            return count;
        }

        public virtual int UnregisterAll() {
            int count = _registers.Count;
            _registers.Clear();
            if (IsHooked) 
                try {
                    IsHooked = Unhook();
                } catch (Exception e) {
                    throw new Exception("Failed to perform unhook. see inner exception.", e);
                }
            return count;
        }

        protected void InvokeKeyPress(BaseKeyboardManager sender, HotkeyEventArgs args) {
            if (KeyPress != null)
                KeyPress(sender, args);
        }

        protected void InvokeKeyDown(BaseKeyboardManager sender, HotkeyEventArgs args) {
            if (KeyDown != null)
                KeyDown(sender, args);
        }

        protected void InvokeKeyUp(BaseKeyboardManager sender, HotkeyEventArgs args) {
            if (KeyUp != null)
                KeyUp(sender, args);
        }

        protected HotkeyEventArgs InvokeKeyPress(BaseKeyboardManager sender, Hotkey args) {
            HotkeyEventArgs ar = null;
            if (KeyPress != null)
                KeyPress(sender, ar = HotkeyEventArgs.Create(args));
            return ar;
        }

        protected HotkeyEventArgs InvokeKeyDown(BaseKeyboardManager sender, Hotkey args) {
            HotkeyEventArgs ar = null;
            if (KeyDown != null)
                KeyDown(sender, ar = HotkeyEventArgs.Create(args));
            return ar;
        }

        protected HotkeyEventArgs InvokeKeyUp(BaseKeyboardManager sender, Hotkey args) {
            HotkeyEventArgs ar = null;
            if (KeyUp != null)
                KeyUp(sender, ar = HotkeyEventArgs.Create(args));
            return ar;
        }

        public void Dispose() {
            UnregisterAll();
        }

    }
}