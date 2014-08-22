﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nucs.Controls {
    public partial class DataGridViewImprCheckBoxCell : DataGridViewCheckBoxCell, ICustomControlEventLayout {
        public DataGridViewImprCheckBoxCell() {
            InitializeComponent();
        }

        protected override void OnContentClick(DataGridViewCellEventArgs e) {
            base.OnContentClick(e);
            OnCellValueChanged(DataGridView, Value);
        }


        protected override void OnContentDoubleClick(DataGridViewCellEventArgs e) {
            base.OnContentDoubleClick(e);
            OnCellValueChanged(DataGridView, Value);
        }

        public event CellChangedHandler CellValueChanged;

        /// <summary>
        /// Invokes CellValueChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        public void OnCellValueChanged(object sender, object value) {
            if (CellValueChanged != null)
                CellValueChanged(sender, value);
        }
    }
}
