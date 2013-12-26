using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using nucs.SystemCore;
using nucs.SystemCore.String;

namespace nucs.Windows.Keyboard {

    /// <summary>
    /// My custom way of handling keys, and yes - Im sick with recreating things. Also this object is immutable
    /// </summary>
    public class AKey : IDisposable {

        #region Properties And Construction

        /// <summary>
        /// Equivalent to <see cref="KeyEventArgs"/>'s KeyCode
        /// </summary>
        public KeyCode Key { get; private set; }
        
        /// <summary>
        /// List of the modifiers
        /// </summary>
        public List<KeyCode> Modifiers { get; private set; }


        /// <summary>
        /// Representation of the <see cref="Key"/> as integer.
        /// </summary>
        public int KeyValue { get { return (int) Key; } }


        /*public AKey(KeyEventArgs args) {
            KeyData = args.KeyData;
            Key = (KeyCode)args.KeyCode;

            ModifiersList = new List<KeyCode>();
            if (args.Control)
                if (KeyCode.LControl.IsKeyDown())
                    ModifiersList.Add(KeyCode.LControl);
                else if (KeyCode.RControl.IsKeyDown())
                    ModifiersList.Add(KeyCode.RControl);
            if (args.Alt)
                if (KeyCode.LMenu.IsKeyDown())
                    ModifiersList.Add(KeyCode.LMenu);
                else if (KeyCode.RMenu.IsKeyDown())
                    ModifiersList.Add(KeyCode.RMenu);
            if (args.Shift)
                if ((KeyCode.LShift.IsKeyDown()))
                    ModifiersList.Add(KeyCode.LShift);
                else if (KeyCode.RShift.IsKeyDown())
                    ModifiersList.Add(KeyCode.RShift);
        }

        public AKey(Keys keyData) : this(new KeyEventArgs(keyData)) {}*/

        public AKey(IEnumerable<KeyCode> modifiers, KeyCode key) {
            Modifiers = modifiers as List<KeyCode> ?? new List<KeyCode>(modifiers);
            Key = key;
        }
       
        #endregion

        #region Methods

        /// <summary>
        /// Converts the modifiers list into a string, for example "RCtrl+LAlt+..."
        /// </summary>
        public string StringifyModifiers() {
            return string.Join("+", Modifiers.Select(k => k.ToString().Replace("Menu", "Alt")));
        }

        /// <summary>
        /// Translates a string such as "RControl+LAlt+..." into a list of Keys {Keys.RControl, Keys.LMenu}
        /// </summary>
        public List<KeyCode> UnstringifyModifiers(string mods) {
            return mods.Split('+').Select(s => Enum.Parse(typeof(KeyCode), s)).Where(o => o != null).Select(o => (KeyCode)o).ToList();
        }

        /// <summary>
        /// Compares the given mods to the current mods list
        /// </summary>
        /// <param name="mods"></param>
        /// <returns></returns>
        public bool CompareModifiers(params KeyCodeModifiers[] mods) {
            return mods.All(m => Modifiers.Contains((KeyCode)m));
        }

        /// <summary>
        /// StringifyModifiers+Key
        /// </summary>
        public override string ToString() {
            return StringifyModifiers() + (Modifiers.Count > 0 ? "+" : "") + Key;
        }

        #endregion

        #region Static

        /// <summary>
        /// Produces a key state from the current keyboard keys and returns it in form of AKey
        /// </summary>
        public static AKey Produce(bool useAsync = true) {
            var l = useAsync
                ? (from KeyCode i in Enumerable.Range(1, 254).Select(i => (KeyCode)i) where (i.IsKeyDownAsync()) select i).ToList()
                : (from KeyCode i in Enumerable.Range(1, 254).Select(i => (KeyCode)i) where (i.IsKeyDown()) select i).ToList();
            var mods = l.Where(k => k.IsSidedModifier()).ToList();
            var key = l.FirstOrDefault(k => k.IsKey());
            return new AKey(mods, key);
        }

        #endregion

        #region Private

        #endregion

        public void Dispose() {}
    }
}
