using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nucs.Controls {


    public partial class StringUpDown : NumericUpDown {
        public event OnSelectionChangedHandler OnSelectionChanged;
        private SortedList<int, string> items;
        public SortedList<int, string> Items { get { return items; } set { } }
        private int selected;
        public int Selected { get { return selected; } set { selected = value; UpdateEditText(); if (OnSelectionChanged != null) OnSelectionChanged(this, new OnSelectionChangedArgs(value, items[value])); } }
        public StringUpDown(ref SortedList<int, string> items) {
            this.items = items;
            InitializeComponent();
        }
        public StringUpDown() {
            items = new SortedList<int, string>{{0,""}};
            InitializeComponent();
        }

        public override void UpButton() {
            selected++;
            if (selected > items.Count-1)
                selected = 0;
            Selected = selected;
        }

        public override void DownButton() {
            selected--;
            if (selected == 0)
                selected = items.Count-1;
            Selected = selected;
        }

        protected override void UpdateEditText() {
            Text = items[selected];
        }

        protected void UpdateItems(SortedList<int, string> ToSet) {
            items = ToSet;
            Selected = 0;
        }

        public override void ResetText() {
            items.Clear();
            items = new SortedList<int, string> { { 0, "" } };
        }


    }

    public delegate void OnSelectionChangedHandler(object sender, OnSelectionChangedArgs args);

    public class OnSelectionChangedArgs {
        public int NewSelectionId;
        public string NewText;
        public OnSelectionChangedArgs(int NewSelectionId, string NewText) {
            this.NewSelectionId = NewSelectionId;
            this.NewText = NewText;
        }
    }
}
