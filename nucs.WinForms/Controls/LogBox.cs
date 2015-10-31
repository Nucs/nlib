using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using nucs.Controls;
using nucs.Forms;
using nucs.Logger;
using nucs.SystemCore.Settings;

namespace nucs.Controls {

    [DebuggerDisplay("LogBox Name={Name} Items={Items.Count}")]
    public partial class LogBox : ListBox, IBaseLogging {
        public event Action LogsCleared;
        public event Action<string> LogAdded;

        /// <summary>
        ///     Returns the logs list, index 0 is the newest item.
        /// </summary>
        public List<string> Logs {get {
            return InvokeRequired ? this.Invoke(() => Logs) : Items.Cast<object>().Select(o => o.ToString()).ToList();
        }
        } 

        public LogBox() {
            InitializeComponent();
            //add to static list for global access.
            LogBoxes.Add(this);
            //Make sure to remove on disposing.
            this.Disposed += (sender, args) => LogBoxes.Remove(this);

            //.Controls.Add(btn);
            var add = new MenuItem("Add Custom Log",
                (sender, args) =>
                    Log(Microsoft.VisualBasic.Interaction.InputBox("Enter the custom log line", "Custom Log Entry")));
            var addtimed = new MenuItem("Add Custom Timed Log", (sender, args) =>
                LogTimed(Microsoft.VisualBasic.Interaction.InputBox("Enter the custom log line", "Custom Log Entry")));

            var copy = new MenuItem("Copy", (sender, args) => {
                if (SelectedIndex == -1) return;
                Clipboard.SetText(Items[SelectedIndex].ToString());
                SystemSounds.Beep.Play();
            });

            var clear = new MenuItem("Delete Selected", (sender, args) => {
                if (SelectedIndex != -1) Items.RemoveAt(SelectedIndex);
            });
            var clearall = new MenuItem("Clear", (sender, args) => ClearLogs());
            ContextMenu = new ContextMenu(new[] {add, addtimed, new MenuItem("-"), copy, clear, clearall});
        }

        /// <summary>
        ///     Reference to the logs holder.
        /// </summary>
        private List<string> _settingsHolder; 

        /// <summary>
        ///     A link to the settings
        /// </summary>
        private ISaveable _settingsSaver; 

        /// <summary>
        ///     Links the logs to a settings field performing a save every time log is inserted.
        /// </summary>
        /// <param name="settings">The AppSettings</param>
        /// <param name="target">The field that holds the logs</param>
        public void LinkToSettings(ISaveable settings, ref List<string> target) {
            _settingsSaver = settings;
            _settingsHolder = target;
            Items.Clear();
            Items.AddRange(target.Cast<object>().ToArray());
        }
        
        private void perform_save() {
            if (_settingsSaver == null) return;
            if (_settingsHolder==null) 
                _settingsHolder = new List<string>(Items.Count);
            else
                _settingsHolder.Clear();
            _settingsHolder.AddRange(Items.Cast<object>().Select(o=>o.ToString()));
            _settingsSaver.Save();
        }

        #region IBaseLogging Implementation
        /// <summary>
        ///     Merges the text with string.join("",text) and then prints it into the logger.
        /// </summary>
        /// <param name="text">The strings that will be joined without space</param>
        public void Log(params string[] text) {
            if (this.InvokeRequired) {
                this.Invoke(() => Log(text));
                return;
            }
            var tolog = text.StringJoin("");
            Items.Insert(0, tolog);
            perform_save();
            if (LogAdded != null)
                LogAdded(tolog);
        }

        /// <summary>
        ///     Log with timestamp, calls <see cref="Log(string[])"/> after preparing a timestamp.
        ///     Recommanded format is [DATE] text
        /// </summary>
        /// <param name="text">The strings that will be joined without space</param>
        public void LogTimed(params string[] text) {
            var msg = string.Format("[{0}] {1}", DateTime.Now.ToString("U", CultureInfo.InvariantCulture), text.StringJoin(""));
            Log(msg);
        }

        /// <summary>
        ///     Clears the logs
        /// </summary>
        /// <returns>Number of logs cleared.</returns>
        public int ClearLogs() {
            var n = Items.Count;
            this.Items.Clear();
            perform_save();
            if (LogsCleared != null)
                LogsCleared();
            return n;
        }
        #endregion

        #region Static Access
        public readonly static List<LogBox> LogBoxes = new List<LogBox>();



        
        #endregion

    }
}

public static class LogToBox {
    /// <summary>
    ///     Merges the text with string.join("",text) and then prints it into the logger.
    /// </summary>
    /// <param name="text">The strings that will be joined without space</param>
    /// <param name="name">The name of the control</param>
    public static bool Log(string name, params string[] text) {
        var logger = LogBox.LogBoxes.FirstOrDefault(l => l.Name.Equals(name));
        if (logger == null) return false;
        logger.Log(text);
        return true;
    }

    /// <summary>
    ///     Log with timestamp, calls <see cref="Log(string,string[])"/> after preparing a timestamp.
    ///     Recommanded format is [DATE] text
    /// </summary>
    /// <param name="text">The strings that will be joined without space</param>
    /// <param name="name">The name of the control</param>
    public static bool LogTimed(string name, params string[] text) {
        var logger = LogBox.LogBoxes.FirstOrDefault(l => l.Name.Equals(name));
        if (logger == null) return false;
        logger.LogTimed(text);
        return true;
    }

    /// <summary>
    ///     Clears the logs
    /// </summary>
    /// <returns>Number of logs cleared.</returns>
    /// <param name="name">The name of the control</param>
    public static int ClearLogs(string name, params string[] text) {
        var logger = LogBox.LogBoxes.FirstOrDefault(l => l.Name.Equals(name));
        if (logger == null) return -1;
        return logger.ClearLogs();
    }
}