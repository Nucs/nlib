namespace nucs.WinForms.Forms
{
    partial class UserConnectionDetailsRequester
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
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnComplete = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.ComboBox();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(10, 19);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtPort.Size = new System.Drawing.Size(38, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "כתובת IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "פורט";
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(80, 46);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(75, 23);
            this.btnComplete.TabIndex = 5;
            this.btnComplete.Text = "אשר פרטים";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(54, 19);
            this.txtIP.Name = "txtIP";
            this.txtIP.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtIP.Size = new System.Drawing.Size(151, 21);
            this.txtIP.TabIndex = 6;
            // 
            // picRefresh
            // 
            
            this.picRefresh.Location = new System.Drawing.Point(211, 18);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(22, 22);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRefresh.TabIndex = 7;
            this.picRefresh.TabStop = false;
            this.picRefresh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picRefresh_MouseDown);
            // 
            // UserConnectionDetailsRequester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 73);
            this.Controls.Add(this.picRefresh);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserConnectionDetailsRequester";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "פרטי התחברות";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.ComboBox txtIP;
        private System.Windows.Forms.PictureBox picRefresh;
    }
}