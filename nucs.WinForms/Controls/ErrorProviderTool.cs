#if !(NET35 || NET3 || NET2)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using Image = System.Drawing.Image;
using ToolTip = System.Windows.Forms.ToolTip;

namespace nucs.WinForms.Controls {
    public class ErrorProviderTool : IDisposable {

        #region Declerations and Constructors

        private readonly List<ErrorInformer> Controls = new List<ErrorInformer>();
        public bool AutoShow { get; set; }
        public Image icon { get; set; }
        public bool RTL { get; set; }
        public ErrorProviderTool() {
            this.AutoShow = true;
            //todo icon = Resources.errorIcon;
            RTL = false;
        }

        public ErrorProviderTool(Image image) {
            this.AutoShow = true;
            icon = image;
            RTL = false;
        }

        public ErrorProviderTool(Image image, bool RTL) {
            this.AutoShow = true;
            icon = image;
            this.RTL = RTL;
        }

        public ErrorProviderTool(bool AutoShow, Image image) {
            this.AutoShow = AutoShow;
            icon = image;
            RTL = false;
        }

        public ErrorProviderTool(bool AutoShow, string pointlessString) {
            this.AutoShow = AutoShow;
            //todo icon = Resources.errorIcon;
            RTL = false;
        }

        public ErrorProviderTool(bool RTL) {
            this.AutoShow = true;
            //todo icon = Resources.errorIcon;
            this.RTL = RTL;
        }

        public ErrorProviderTool(bool RTL, bool AutoShow) {
            this.AutoShow = AutoShow;
            //todo icon = Resources.errorIcon;
            this.RTL = RTL;
            this.AutoShow = AutoShow;
        }

        public ErrorProviderTool(bool RTL, bool AutoShow, Image image) {
            this.RTL = RTL;
            this.AutoShow = AutoShow;
            icon = image;
        }

        public ErrorProviderTool(bool AutoShow, IEnumerable<ErrorInformer> errorsList) {
            Controls.AddRange(errorsList);
            this.AutoShow = AutoShow;
            RTL = false;
        }

        #endregion
        #region Methods

        public void Add(ErrorInformer errorInformer) {
            Controls.Add(errorInformer);
            errorInformer.Visibile = AutoShow;
        }

        public void Add(Control control, string errorText, bool _RTL) {
            ErrorInformer err;
            Controls.Add(err = new ErrorInformer(control, icon, errorText, _RTL));
            err.Visibile = AutoShow;
        }
        public void Add(Control control, string errorText) {
            ErrorInformer err;
            Controls.Add(err = new ErrorInformer(control, icon, errorText, RTL));
            err.Visibile = AutoShow;
        }
        public void Add(Control control, Image ico, string errorText, bool _RTL) {
            ErrorInformer err;
            Controls.Add(err = new ErrorInformer(control, ico, errorText, _RTL));
            err.Visibile = AutoShow;
        }

        public void Add(Control control, Image icon, string errorText) {
            ErrorInformer err;
            Controls.Add(err = new ErrorInformer(control, icon, errorText, RTL));
            err.Visibile = AutoShow;
        }

        /// <summary>
        /// Clears the errors list and prepares for new errors
        /// </summary>
        public void Reset() {
            foreach (ErrorInformer err in Controls) 
                err.Dispose();
            Controls.Clear();

        }

        public void ShowAll() {
            Controls.AsParallel().ForAll(err => err.Show());   
        }

        public void HideAll() {
            Controls.AsParallel().ForAll(err => err.Hide());
        }
        /// <summary>
        /// Amount of errors in this current season
        /// </summary>
        public int Count { get { return Controls.Count; } }

        public void Dispose() {
            Reset();
            icon.Dispose();
        }

        #endregion
        #region public class ErrorInformer {
        public class ErrorInformer : IDisposable {

            public bool Visibile { set { if (value) Show(); else Hide(); } }

            public readonly PictureBox ErrorPicture;
            public bool RTL { get; set; }
            public Control Control { get; set; }
            public ToolTip Tooltip { get; set; }
            public string ErrorText { get; set; }
            private const int DEFAULT_PADDING = 5;

            public ErrorInformer(Control control, Image image, string errorText, bool RTL) {
                ErrorPicture = new PictureBox { Image = image, Size = new Size(17,17), SizeMode = PictureBoxSizeMode.StretchImage};
                Tooltip = new ToolTip();
                if (errorText != string.Empty)
                    Tooltip.SetToolTip(ErrorPicture, errorText);
                this.RTL = RTL;
                this.Control = control;
                this.ErrorText = errorText;
                control.Parent.Controls.Add(ErrorPicture);

                if (RTL)
                    ErrorPicture.Location = new Point(
                        control.Location.X + DEFAULT_PADDING*-1 + ErrorPicture.Size.Width*-1,
                        control.Location.Y + (control.Size.Height-ErrorPicture.Size.Height)/2);
                else
                    ErrorPicture.Location = new Point(
                        control.Location.X + DEFAULT_PADDING + control.Size.Width,
                        control.Location.Y + (control.Size.Height - ErrorPicture.Size.Height) / 2);
            }


            private Timer timer;
            public void Show() {
                count = 0;
                if (timer != null)
                    timer.Dispose();
                timer = new Timer {Interval = 350, Enabled = false, Tag = ErrorPicture};
                timer.Interval = 350;
                timer.Tick +=timer_Tick;
                timer.Enabled = true;
            }

            private int count = 0;
            private void timer_Tick(object sender, EventArgs e) {
                if (count < 3*2) {
                    count++;
                    (timer.Tag as PictureBox).Visible = !(timer.Tag as PictureBox).Visible;
                    return;
                }
                timer.Enabled = false;
                (timer.Tag as PictureBox).Visible = true;
            }

            public void Hide() {
                if (timer!=null)
                    timer.Dispose();
                timer = null;
                ErrorPicture.Hide();
            }

            public void Dispose() {
                ErrorPicture.Hide();
                ErrorPicture.Dispose();
                Control = null;
                Tooltip.Dispose();
                if (timer != null)
                    timer.Dispose();

            }
        }
        #endregion



    }
}
#endif