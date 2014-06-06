/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace nucs.Windows.Keyboard {
    /// <summary>
    /// GlobalHotkeyRegisterator is a class, used to manage global key registeration for a single application.
    /// 
    /// </summary>
    public sealed class oldGlobalHotkeyRegisterator : IDisposable {
        public const int WM_HOTKEY = 0x0312;

        private readonly List<HotkeyReg> RegisterationsLog = new List<HotkeyReg>();

        /// <summary>
        /// Register a global hotkey, that can be pressed from anywhere and will be received in the form of the handle wndproc function.
        /// </summary>
        /// <param name="key">The key to invoke a send in wndproc</param>
        /// <param name="uId">unique number to represent the registeration, can be random but make sure there won't be any duplications</param>
        /// <param name="handle">Form's handle to register an event to the form's wndproc event</param>
        /// <param name="Description">Describe the key binding. This also can aid in later search for the key.</param>
        /// <returns>Successful or not</returns>
        public bool Register(Keys key, int uId, IntPtr handle, string Description = "") {
            return Register(null, key, uId, handle);
        }

        /// <summary>
        /// Register a global hotkey, that can be pressed from anywhere and will be received in the form of the handle wndproc function.
        /// </summary>
        /// <param name="modifiers">Represent buttons such as Ctrl, Alt Shift</param>
        /// <param name="key">The key to invoke a send in wndproc</param>
        /// <param name="uId">unique number to represent the registeration, can be random but make sure there won't be any duplications</param>
        /// <param name="handle">Form's handle to register an event to the form's wndproc event</param>
        /// <param name="Description">Describe the key binding. This also can aid in later search for the key.</param>
        /// <returns>Successful or not</returns>
        public bool Register(IEnumerable<ModifierKeys> modifiers, Keys key, int uId, IntPtr handle, string Description = "") {
            var hk = Add((modifiers ?? new[] { ModifierKeys.None }).ToArray(), key, uId, handle, Description);
            return hk.Register();
        }

        /// <summary>
        /// Unregisters a hotkey, based on a key and where there are no modifiers in particular.
        /// </summary>
        /// <returns>Successful or not</returns>
        public bool Unregister(Keys key) {
            var hk = RegisterationsLog.FirstOrDefault(k => k.Key == key && k.ModifiersKeys[0] == ModifierKeys.None);
            return hk != null && hk.Unregister();
        }

        public bool Unregister(string description) {
            var hk = RegisterationsLog.FirstOrDefault(k => k.Description == description);
            return hk != null && hk.Unregister();
        }
        /// <summary>
        /// Unregisters a hotkey, based on the key and the modifiers
        /// </summary>
        /// <returns>Successful or not</returns>
        public bool Unregister (ModifierKeys[] modifiers, Keys key) {
            var hk = RegisterationsLog.FirstOrDefault(k => k.Key == key && !modifiers.Distinct().Except(k.ModifiersKeys).Any());
            return hk != null && hk.Unregister();
        }

        private HotkeyReg Add(ModifierKeys[] modifiers, Keys key, int uId, IntPtr handle, string Description) {
            var hk = new HotkeyReg(modifiers, key, uId, handle, Description);
            RegisterationsLog.Add(hk);
            return hk;
        }

        public IEnumerable<HotkeyReg> Registerations() {
            return RegisterationsLog.ToArray();
        }


        public void UnregisterAll() {
            RegisterationsLog.ForEach(r=>r.Unregister());
        }

        public void Clear() {
            UnregisterAll();
            RegisterationsLog.Clear();
        }

        public void Dispose() {
            UnregisterAll();
            RegisterationsLog.Clear();
        }


        


    }
    /// <summary>
    /// Represents a Hotkey Registeration, this can be used without GlobalHotkeyRegistartor. but use with caution
    /// </summary>
    public sealed class HotkeyReg {
        public string Description { get; set; }
        public bool Registered { get; set; }
        public int UniqueId { get; set; }
        public IntPtr Handle { get; set; }
        public ModifierKeys[] ModifiersKeys { get; set; }
        public uint Modifiers { get { return ModifiersTools.MergeModifiers(ModifiersKeys); } }
        public Keys Key { get; set; }
        public uint VKey { 
            get { 
                if (Key == Keys.None || Key == Keys.NoName) 
                    return 0;
                return (uint) Key;
            }
        }

        public HotkeyReg(ModifierKeys[] modifiers, Keys key, int uId, IntPtr handle, string Description = "") {
            UniqueId = uId;
            Handle = handle;
            this.Description = Description;
            if (modifiers == null || modifiers.Length == 0)
                modifiers = new[] {ModifierKeys.None};
            ModifiersKeys = modifiers.Distinct().ToArray();
            Key = key;
        }

        public HotkeyReg(Keys key, int uId, IntPtr handle, string Description = "") {
            UniqueId = uId;
            Handle = handle;
            this.Description = Description;
            ModifiersKeys = new [] {ModifierKeys.None};
            Key = key;
        }

        public bool Register() {
            try {
                return Registered = NativeWin32.RegisterHotKey(Handle, UniqueId, Modifiers, VKey);
            } catch {
                return Registered = false;
            }
        }

        public bool Unregister() {
            try {
                return Registered = NativeWin32.UnregisterHotKey(Handle, UniqueId);
            } catch {
                return Registered = false;
            }
        }

        public override string ToString() {
            var b = new StringBuilder();
            if (ModifiersKeys.Contains(ModifierKeys.Control)) b.Append("C");
            if (ModifiersKeys.Contains(ModifierKeys.Alt)) b.Append("A");
            if (ModifiersKeys.Contains(ModifierKeys.Shift)) b.Append("S");
            if (b.Length != 0)
                b.Append('-');
            b.Append(Key.ToString());
            return b.ToString();
        }

    }

}
*/
