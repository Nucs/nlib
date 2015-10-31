using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using nucs.SystemCore.String;
using ProtoBuf;

namespace nucs.Windows.Keyboard {
    /// <summary>
    /// An alternative and easier way to determine modifier keys.
    /// By default the mods are both sided unless you add a RKey or LKey to them.
    /// Be careful from combining RKeys and LKeys, you might end up with both sides to be allowed.
    /// In order to compare two ModKeys, use the extension <see cref="ModKeysExtensions.Compare"/>
    /// </summary>
    [Flags]
    
    public enum ModKeys : int {
        None = 0,
        /// <summary>
        /// Flag as Right sided key.
        /// </summary>
        RKey = 1,
        /// <summary>
        /// Flag as Left sided key.
        /// </summary>
        LKey = 2,
        /// <summary>
        /// Marks the key as both sides, for both sides either flag this to the modifier or no sides at all. (not RKey and LKey).
        /// </summary>
        RLKey = 3,
        Control=4,
        /// <summary>
        /// Equivalent to Menu.
        /// </summary>
        Alt=8,
        Menu=8,
        Shift=16,

        //Combinations:
        RControl = Control | RKey,
        LControl = Control | LKey,
        RAlt = Alt | RKey,
        RMenu = RAlt,
        LAlt = Alt | LKey,
        LMenu = LAlt,
        RShift = Shift | RKey,
        LShift = Shift | LKey,
    }

    public static class ModKeysExtensions {

        /// <summary>
        /// Returns which side is this modifier.
        /// incase of both <see cref="ModKeys.RLKey"/> is returned.
        /// otherwise, the side is returned.
        /// </summary>
        /// <param name="mk">The modifier key</param>
        /// <returns></returns>
        public static ModKeys ExportSide(this ModKeys mk) {
            int side = ((int)mk & 0x03);
            if (side == 0x03 || side == 0) //is it both sides?
                return ModKeys.RLKey;
            return (ModKeys) side; //let casting determine which.
        }

        public static ModKeys ToModKeys(this KeyCode kc) {
            var res = ModKeys.None;
            var str = kc.ToString();
            if (str.ContainsAny("Control", "Alt", "Menu", "Shift") == false) return ModKeys.None;
            res |= str.StartsWith("R") ? ModKeys.RKey : (str.StartsWith("L") ? ModKeys.LKey : 0);
            
            return res | (ModKeys) Enum.Parse(typeof (ModKeys), str.StartsWithAny("R", "L") ? str.Substring(1, str.Length-1) : str);
        }

        public static ModKeys ToModKeys(this IEnumerable<KeyCode> kc) {
            var kk = kc.ToList();
            return kk.Count==0 ? ModKeys.None : kk.Select(k => k.ToModKeys()).Aggregate((a, b) => a | b);
        }

        /// <summary>
        /// Compares two <see cref="ModKeys"/>. Use this instead of == or .Equals!!
        /// </summary>
        public static bool Compare(this ModKeys mk, ModKeys to) {
            var mk_side = mk.ExportSide();
            var to_side = to.ExportSide();
            if (mk_side != ModKeys.RLKey && to_side != ModKeys.RLKey && mk_side != to_side) return false;
            return ((int) mk & ~3) == ((int) to & ~3); //compare with elimination of sides.
        }


    }

}