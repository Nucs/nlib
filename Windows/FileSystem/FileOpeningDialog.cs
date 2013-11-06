using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using nucs.Windows.FileSystem;

namespace nucs.Windows {
    public static class FileOpeningDialog {
        public static FileDialogResult Show(IWin32Window Parent = null, bool InHebrew = false) {
            using (var dialog = new FileOpeningDialogForm(InHebrew)) {
                dialog.ShowDialog(Parent);
                return dialog.Result;
            }
        }
    }

    public enum FileDialogResult {
        Open,
        Save,
        SaveAndOpen,
        Cancel
    }
}
