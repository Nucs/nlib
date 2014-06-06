/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace nucs.Windows.Keyboard {
    public static class ModifiersTools {
       public static uint MergeModifiers(params ModifierKeys[] Keys) {
            var keys = Keys.Distinct().ToArray();
            if (keys.Length == 0 || ((keys.All(j => j == ModifierKeys.None)))) return (uint)ModifierKeys.None;
            uint s = 0;
            foreach (var k in keys.Where(j => j != ModifierKeys.None)) {
                if (s == 0)
                    s = (uint)k;
                else
                    s |= (uint)k;
            }
            return s;
        }

        public static uint MergeModifiers(params Keys[] Keys) {
            var keys = Keys.Distinct().ToArray();
            if (keys.Length == 0 || ((keys.All(j => j == Keys.)))) return (uint)ModifierKeys.None;
            uint s = 0;
            foreach (var k in keys.Where(j => j != Keys.None))
            {
                if (s == 0)
                    s = (uint)k;
                else
                    s |= (uint)k;
            }
            return s;
        }


/*        public static uint MergeModifiers(params Keys[] Keys) {
            var keys = Keys.Where(k => (k.ToString().Contains("Control")
                                       || k.ToString().Contains("Alt")
                                       || k.ToString().Contains("Shift")
                                       || k.ToString().Contains("Windows")) && !k.ToString().Contains("Key")).ToArray();
            return (Control.ModifiersKeys 
        }#1#


    }
}
*/
