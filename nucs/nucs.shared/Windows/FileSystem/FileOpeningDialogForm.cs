/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace nucs.Windows.FileSystem {
    internal partial class FileOpeningDialogForm : Form {
        public FileOpeningDialogForm(bool InHebrew) {
            InitializeComponent();
            Closing += (sender, args) => {
                if (PlannedShutdown == false) {
                    Result = FileDialogResult.Cancel;
                }
            };

            if (InHebrew == false) {
                btnClose.Text = "Close";
                btnOpen.Text = "Open";
                btnSave.Text = "Save";
                btnSaveOpen.Text = "Save and Open";
                lblHebrew.Hide();
                pictureBox1.Location = new Point(213, 19);
            } else {
                lblEnglish.Hide();
            }

        }

        internal FileDialogResult Result;
        private bool PlannedShutdown = false;
        private void FileOpeningDialogForm_Load(object sender, EventArgs e) {

        }

        private void btnSave_Click(object sender, EventArgs e) {
            Result = FileDialogResult.Save;
            PlannedShutdown = true;
            Close();
        }

        private void btnOpen_Click(object sender, EventArgs e) {
            Result = FileDialogResult.Open;
            PlannedShutdown = true;
            Close();
        }

        private void btnSaveOpen_Click(object sender, EventArgs e) {
            Result = FileDialogResult.SaveAndOpen;
            PlannedShutdown = true;
            Close();

        }

        private void btnClose_Click(object sender, EventArgs e) {
            Result = FileDialogResult.Cancel;
            PlannedShutdown = true;
            Close();
        }
    }
}
*/
