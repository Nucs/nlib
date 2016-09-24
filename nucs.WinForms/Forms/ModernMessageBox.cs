using System;
using System.Threading.Tasks;
using System.Windows.Forms;
#if (NET35 || NET3 || NET2)
using nucs.Mono.System.Threading;
#else

#endif
namespace nucs.WinForms.Forms {
    public static class MessageBoxy {
        /// <summary>
        /// Sorter version of MessageBox.Show, supporting rtl
        /// </summary>
        /// <returns><see cref="DialogResult"/>urns></returns>
        public static DialogResult Show(string text, string title="", MessageBoxButtons buttons=MessageBoxButtons.OK, MessageBoxIcon icon=MessageBoxIcon.None, bool rtl = false) {
            return MessageBox.Show(text, title, buttons, icon, MessageBoxDefaultButton.Button1, rtl ? MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0);
        }

        /// <summary>
        /// Sorter version of MessageBox.Show, supporting rtl
        /// </summary>
        /// <returns><see cref="DialogResult"/>urns></returns>
        public static Task<DialogResult> ShowAsync(string text, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, bool rtl = false) {
#if NET4_5
            return Task.Run(()=> MessageBox.Show(text, title, buttons, icon, MessageBoxDefaultButton.Button1, rtl ? MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0));
#elif NET4_0
            return Task.Factory.StartNew(()=>MessageBox.Show(text, title, buttons, icon, MessageBoxDefaultButton.Button1, rtl ? MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0));
#else
            throw new InvalidOperationException("This method is unavailable for current .net version.");
#endif
        }
    }
}
