namespace nucs.WinForms.WindowFinder
{
    partial class WindowFinder
    {
        /// <summary>
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picTarget = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // picTarget
            // 
            this.picTarget.BackColor = System.Drawing.Color.White;
            this.picTarget.Location = new System.Drawing.Point(0, 0);
            this.picTarget.Name = "picTarget";
            this.picTarget.Size = new System.Drawing.Size(92, 104);
            this.picTarget.TabIndex = 0;
            this.picTarget.TabStop = false;
            this.picTarget.Click += new System.EventHandler(this.picTarget_Click);
            this.picTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseDown);
            this.picTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseMove);
            this.picTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseUp);
            // 
            // WindowFinder
            // 
            this.Controls.Add(this.picTarget);
            this.Name = "WindowFinder";
            this.Size = new System.Drawing.Size(272, 216);
            this.Load += new System.EventHandler(this.WindowFinder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picTarget)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.PictureBox picTarget;
    }
}
