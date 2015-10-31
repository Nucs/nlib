using System;
using System.Windows.Forms;

namespace nucs.WinForms.Controls
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
