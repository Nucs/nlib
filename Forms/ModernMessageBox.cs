using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nucs.Forms {
    public static class MessageBoxy {
        /// <summary>
        /// Sorter version of MessageBox.Show, supporting rtl
        /// </summary>
        /// <returns><see cref="DialogResult"/>urns></returns>
        public static DialogResult Show(string text, string title="", MessageBoxButtons buttons=MessageBoxButtons.OK, MessageBoxIcon icon=MessageBoxIcon.None, bool rtl = false) {
            return MessageBox.Show(text, title, buttons, icon, MessageBoxDefaultButton.Button1, rtl ? MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0);
        }
    }
}
