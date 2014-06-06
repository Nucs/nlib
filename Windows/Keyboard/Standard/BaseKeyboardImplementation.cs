using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nucs.Collections;

namespace nucs.Windows.Keyboard {
    public abstract class BaseKeyboardManager : IDisposable {

        protected readonly ImprovedList<Hotkey> _registers = new ImprovedList<Hotkey>();

        public IReadOnlyCollection<Hotkey> Registers {
            get { return _registers.AsReadOnly(); }
        }

        public event KeyPressEventHandler HotkeyPressed;
        public abstract bool IsStarted { get; protected set; }

        /// <summary>
        /// Hooks or begins the listen
        /// </summary>
        public abstract void Hook();
        public abstract void Unhook();

        public virtual Hotkey Register(Keys key, Keys modifiers, string description) {
            if (_registers.Any(hkk => string.Equals(hkk.Description, description) || (hkk.Modifiers == modifiers && hkk.Key == key)))
                return null;
            Hotkey hk = Hotkey.Create(key, modifiers, description);
            _registers.Add(hk);
            return hk;
        }

        public virtual Hotkey Register(Keys key, string description) { return Register(key, Keys.None, description); }

        public bool Unregister(Keys key) {
            return Unregister(key, Keys.None);
        }

        public abstract bool Unregister(Keys key, Keys modifier);
        public abstract bool Unregister(string description);
        public abstract bool UnregisterAll();

        public virtual void Clear() {
            UnregisterAll();
            _registers.Clear();
        }

        protected void InvokeHotKeyPressed(HotkeyEventArgs args) {
            if (HotkeyPressed != null)
                HotkeyPressed(args);
        }

        public void Dispose() {
            UnregisterAll();
            Clear();
        }

    }
}