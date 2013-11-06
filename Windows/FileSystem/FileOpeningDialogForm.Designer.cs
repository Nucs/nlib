namespace nucs.Windows.FileSystem
{
    partial class FileOpeningDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileOpeningDialogForm));
            this.btnSaveOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lblHebrew = new System.Windows.Forms.Label();
            this.lblEnglish = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSaveOpen
            // 
            this.btnSaveOpen.Location = new System.Drawing.Point(111, 62);
            this.btnSaveOpen.Name = "btnSaveOpen";
            this.btnSaveOpen.Size = new System.Drawing.Size(87, 23);
            this.btnSaveOpen.TabIndex = 0;
            this.btnSaveOpen.Text = "שמור ופתח";
            this.btnSaveOpen.UseVisualStyleBackColor = true;
            this.btnSaveOpen.Click += new System.EventHandler(this.btnSaveOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(18, 62);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "שמור";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(111, 91);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(18, 91);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(87, 23);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "פתח";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lblHebrew
            // 
            this.lblHebrew.AutoSize = true;
            this.lblHebrew.Font = new System.Drawing.Font("Calibri", 7.75F);
            this.lblHebrew.Location = new System.Drawing.Point(12, 29);
            this.lblHebrew.Name = "lblHebrew";
            this.lblHebrew.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblHebrew.Size = new System.Drawing.Size(214, 13);
            this.lblHebrew.TabIndex = 4;
            this.lblHebrew.Text = "מה ברצונך לעשות עם לעשות עם הקובץ?";
            this.lblHebrew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEnglish
            // 
            this.lblEnglish.AutoSize = true;
            this.lblEnglish.Location = new System.Drawing.Point(13, 29);
            this.lblEnglish.Name = "lblEnglish";
            this.lblEnglish.Size = new System.Drawing.Size(191, 13);
            this.lblEnglish.TabIndex = 5;
            this.lblEnglish.Text = "What would you like to do with the file?";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(230, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 33);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // FileOpeningDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(301, 125);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblEnglish);
            this.Controls.Add(this.lblHebrew);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSaveOpen);
            this.Font = new System.Drawing.Font("Calibri", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileOpeningDialogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FileOpeningDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label lblHebrew;
        private System.Windows.Forms.Label lblEnglish;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}