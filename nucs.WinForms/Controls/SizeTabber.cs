using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using nucs.SystemCore.String;

namespace nucs.WinForms.Controls {
    public partial class SizeTabber : TabControl {
        public readonly Form MainForm;
        public SizeTabber(Form MainForm, Size initialSize) {
        
            InitializeComponent();
            this.MainForm = MainForm;
        }
        public SizeTabber(Form MainForm, int width, int height) : this (MainForm, new Size(width, height)) {}
        public readonly List<Control> AddedControls = new List<Control>(); 
        protected override void OnControlAdded(ControlEventArgs e) {
 	        base.OnControlAdded(e);
            AddedControls.Add(e.Control);
            e.Control.Tag = MainForm.Width + ":" + MainForm.Height;
        }

        public void SetControlCustomSize(Control control, Size size) {
            control.Tag = size.Width + ":" + size.Height;
        }

        public Size GetControlCustomSize(Control control) {
            var size = new Size();
            try {
                var locs = control.Tag.ToString().Split(':');
                size.Width = locs[0].ToInt32();
                size.Height = locs[1].ToInt32();
            } catch {}
            return size;
        }

        protected override void OnSelected(TabControlEventArgs e) {
            base.OnSelected(e);
            Console.WriteLine("page:"+e.TabPage.Name +" index:"+e.TabPageIndex);
            Size size;
            Console.WriteLine(size = GetControlCustomSize(Controls.Find(e.TabPage.Name, false).FirstOrDefault()));
            MainForm.Size = size;


        }
    }
}
