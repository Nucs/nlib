using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nucs.Collections.Extensions;

namespace nucs.Controls {

    public delegate void SelectionChangedHandler(CheckBox c, bool state);
    /// <summary>
    /// Binds multiple checkboxes together into a single checked list. also thread-safe.
    /// </summary>
    public class CheckBoxConnector : Control {
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();
        public event SelectionChangedHandler SelectionChanged;
        private CheckBoxConnector(IEnumerable<CheckBox> boxes) {
            _checkBoxes.AddRange(boxes.Where(cb=>cb != null));
            //prepare and make sure only 1 is checked b4 binding the event
            if (_checkBoxes.All(c => !c.Checked) || _checkBoxes.Count(c => c.Checked) > 1) {
                _checkBoxes.ForEach(c => _check(c, false));
                _check(_checkBoxes[0], true);
            }
            _checkBoxes.ForEach(box => box.CheckedChanged += OnChecked);
            Disposed += (sender, args) => onDisposing();
        }

        /// <summary>
        /// Returns the checked checkbox
        /// </summary>
        public CheckBox Checked {
            get { return _checkBoxes.First(c => c.Checked); }
        }

        /// <summary>
        /// The checkboxes that are bound together
        /// </summary>
        public ReadOnlyCollection<CheckBox> CheckBoxes {
            get { return _checkBoxes.AsReadOnly(); }
        }

        private void OnChecked(object sender, EventArgs e) {
            var activeCheckBox = sender as CheckBox;
            if (activeCheckBox == null) return;
            foreach (var c in _checkBoxes) {
                c.CheckedChanged -= OnChecked;
                if (!c.Equals(activeCheckBox)) {
                    if (c.Checked)
                        _check(c, false);
                } else
                    _check(c, true);
                if (SelectionChanged != null)
                    SelectionChanged(activeCheckBox, activeCheckBox.Checked);
                c.CheckedChanged += OnChecked;
            }
        }

        private void onDisposing() {
            foreach (var cb in _checkBoxes) {
                if (cb == null || cb.IsDisposed) continue;
                cb.CheckedChanged -= OnChecked;
            }
            _checkBoxes.Clear();
        }

        private void _check(CheckBox c, bool val) {
            if (c.InvokeRequired)
                c.Invoke(new MethodInvoker(() => c.Checked = val));
            else
                c.Checked = val;
        }

        /// <summary>
        /// Binds multiple checkboxes together into a single checked list. also thread-safe.
        /// </summary>
        public static CheckBoxConnector BindTogether(params CheckBox[] boxes) {
            return new CheckBoxConnector(boxes);
        }

    }
}
