#if !AV_SAFE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Windows.Input;
using nucs.Settings;
using nucs.SystemCore;
using nucs.SystemCore.String;
using Z.ExtensionMethods.Object;

namespace nucs.Windows.Keyboard {

    /// <summary>
    /// My custom way of handling keys, and yes - Im sick with recreating things. Also this object is immutable
    /// </summary>
    [Serializable]
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

        [Obsolete("Used for deserialization")]
        public AKey() {

        }

        public AKey(IEnumerable<KeyCode> modifiers, KeyCode key) {
            Modifiers = modifiers as List<KeyCode> ?? new List<KeyCode>(modifiers);
            Key = key;
        }
       
        public AKey(KeyCode key) {
            Modifiers = new List<KeyCode>(0);
            Key = key;
        }

        public AKey(string modifiers, string key) {
            Modifiers = UnstringifyModifiers(modifiers);
            Key = (KeyCode)Enum.Parse(typeof (KeyCode), key);
        }

        public AKey(string key) {
            Modifiers = new List<KeyCode>(0);
            Key = (KeyCode)Enum.Parse(typeof (KeyCode), key);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the modifiers list into a string, for example "RCtrl+LAlt+..."
        /// </summary>
        public string StringifyModifiers() {
            return string.Join("+", (Modifiers = (Modifiers ?? new List<KeyCode>(0))).Select(k => k.ToStringSafe().Replace("Menu", "Alt")).ToArray());
        }

        /// <summary>
        /// Translates a string such as "RControl+LAlt+..." into a list of Keys {Keys.RControl, Keys.LMenu}
        /// </summary>
        public static List<KeyCode> UnstringifyModifiers(string mods) {
            return mods.ToStringSafe().Split('+').Select(s => string.IsNullOrEmpty(s) ? KeyCode.None : Enum.Parse(typeof(KeyCode), s)).Where(o => o != null).Select(o => (KeyCode)o).ToList();
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
        /// Compares the given mods to the current mods list if any of them is contained
        /// </summary>
        /// <param name="mods"></param>
        /// <returns></returns>
        public bool ContainsAnyModifiers(params KeyCodeModifiers[] mods) {
            return Modifiers.Any(m => mods.Any(mm=>mm.Equals(m)));
        }

        /// <summary>
        /// StringifyModifiers+Key
        /// </summary>
        public override string ToString() {
            return StringifyModifiers() + (Modifiers.Count > 0 ? "+" : "") + Key;
        }

        /// <summary>
        /// Converts from string or parses..
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static AKey FromString(string s) {
            s = s.Trim();
            if (string.IsNullOrEmpty(s))
                return new AKey(KeyCode.None);
            var li = s.LastIndexOf('+');
            if (li == -1) 
                return new AKey(s);
            return new AKey(s.Substring(0, li), s.Substring(li, s.Length));
        }

        #endregion

        #region Static

        /// <summary>
        /// Produces a key state from the current keyboard keys and returns it in form of AKey
        /// </summary>
        public static AKey Produce(bool useAsync = true) {
            var l = useAsync
                ? (from KeyCode i in Enumerable.Range(1, 256).Select(i => (KeyCode)i) where (i.IsKeyDownAsync()) select i).ToList()
                : (from KeyCode i in Enumerable.Range(1, 256).Select(i => (KeyCode)i) where (i.IsKeyDown()) select i).ToList();
            var mods = l.Where(k => k.IsSidedModifier()).ToList();
            var key = l.FirstOrDefault(k => k.IsKey());
            return new AKey(mods, key);
        }

        /// <summary>
        /// Produces a key state from the current keyboard keys and returns it in form of AKey
        /// </summary>
        internal static AKey Produce(KeyCode key, List<KeyCode> mods) {
            return new AKey(mods, key);
        }

        #endregion

        public void Dispose() {}
    }
    
    [SettingsConverter]
    public class AKeyJSConverter : JavaScriptConverter {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            //return AKey.FromString(dictionary.FirstOrDefault().Value.ToStringSafe());
            return new AKey(dictionary["mods"].ToStringSafe(), dictionary["key"].ToStringSafe());
        }

        
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            var k = obj as AKey;
            var dic = new Dictionary<string, object>(2) {{"mods", k.StringifyModifiers()}, {"key", k.Key.ToString()}};
            return dic;
        }

        public override IEnumerable<Type> SupportedTypes {
            get { return new []{typeof(AKey)};}
        }
    }
}
#endif