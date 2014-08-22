using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nucs.Controls
{
    public partial class DataGridViewImprCheckBoxColumn : DataGridViewCheckBoxColumn {
        public DataGridViewImprCheckBoxColumn(DataGridView dgv) {
            InitializeComponent();
        }

        private readonly DataGridViewCell tmplate = new DataGridViewImprCheckBoxCell();
        public override DataGridViewCell CellTemplate {
            get {
                return tmplate;
            }

            set {
                if (value != null && !(value is DataGridViewImprCheckBoxCell)) {
                    throw new InvalidCastException("Invalid Template cast!");
                }
                base.CellTemplate = value;
            } 

        }
    }
}
