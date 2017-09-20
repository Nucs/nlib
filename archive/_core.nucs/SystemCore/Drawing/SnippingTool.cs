/*using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Media;
using System.Windows.Forms;
using BotSuite;
using ImageProcessorCore;
using nucs.Windows;
using nucs.Windows.Hooking;

namespace nucs.SystemCore.Drawing {

    /// <summary>
    /// This class is used to take a photo from part of the screen. use the static function <see cref="SnippingTool.Snip(PixelFormat)"/> or it's overloads.
    /// </summary>
    public sealed partial class SnippingTool : Form {
        /// <summary>
        /// Takes screenshot of the screen (supports multiple monitors) and then lets user to select the wanted area and returns that area.
        /// </summary>
        public static Image Snip(PixelFormat format = PixelFormat.Format24bppRgb) {
            MultiScreenSize m_MultiScreenSize = FindMultiScreenSize();

            var bmp = new Bitmap(m_MultiScreenSize.maxRight - m_MultiScreenSize.minX, m_MultiScreenSize.maxBottom - m_MultiScreenSize.minY, format);
            Graph = Graphics.FromImage(bmp);
            Graph.SmoothingMode = SmoothingMode.None;

            BitmapSize = bmp.Size;



            using (var snipper = new SnippingTool(bmp)) {
                snipper.Location = new Point(m_MultiScreenSize.minX, m_MultiScreenSize.minY);

                if (snipper.ShowDialog() == DialogResult.OK) {
                    return snipper.Image;
                }
            }

            return null;
        }

        /// <summary>
        /// Takes screenshot of the window (supports multiple monitors) and then lets user to select the wanted area and returns that area.
        /// </summary>
        public static Image Snip(IntPtr hWnd, PixelFormat format = PixelFormat.Format24bppRgb) {
            NativeWin32.SetForegroundWindow(hWnd);
            var p = NativeWin32.GetWindowRect(hWnd);
            var bmp = ScreenShot.Create(hWnd);
            Graph = Graphics.FromImage(bmp);
            Graph.SmoothingMode = SmoothingMode.None;
            
            using (var snipper = new SnippingTool(bmp) {SpecificWindowMode = true}) {
                snipper.Location = new Point(p.Left, p.Top);
                NativeWin32.SetForegroundWindow(snipper.Handle);
                 
                if (snipper.ShowDialog() == DialogResult.OK) {
                    return snipper.Image;
                }
            }

            return null;
        }

        /// <summary>
        /// Takes screenshot of the window (supports multiple monitors) and then lets user to select the wanted area and returns that area.
        /// Also returns the rectangle of the selected part inside of the window.
        /// </summary>
        public static Image Snip(IntPtr hWnd, out Rectangle rect, PixelFormat format = PixelFormat.Format24bppRgb) {
            NativeWin32.SetForegroundWindow(hWnd);
            var p = NativeWin32.GetWindowRect(hWnd);
            var bmp = ScreenShot.Create(hWnd);
            Graph = Graphics.FromImage(bmp);
            Graph.SmoothingMode = SmoothingMode.None;
            
            using (var snipper = new SnippingTool(bmp) {SpecificWindowMode = true}) {
                snipper.Location = new Point(p.Left, p.Top);
                NativeWin32.SetForegroundWindow(snipper.Handle);
                 
                if (snipper.ShowDialog() == DialogResult.OK) {
                    rect = snipper.rcSelect;
                    return snipper.Image;
                }
            }
            rect = Rectangle.Empty;
            return null;
        }

        /// <summary>
        /// Takes screenshot of the screen (supports multiple monitors) and then lets user to select the wanted area and returns that area.
        /// Also returns the rectangle of the selected part inside of the window.
        /// </summary>
        public static Image Snip(out Rectangle rect, PixelFormat format = PixelFormat.Format24bppRgb) {
            MultiScreenSize m_MultiScreenSize = FindMultiScreenSize();
            var bmp = new Bitmap(m_MultiScreenSize.maxRight - m_MultiScreenSize.minX, m_MultiScreenSize.maxBottom - m_MultiScreenSize.minY, format);
            Graph = Graphics.FromImage(bmp);
            Graph.SmoothingMode = SmoothingMode.None;
            BitmapSize = bmp.Size;
            using (var snipper = new SnippingTool(bmp)) {
                snipper.Location = new Point(m_MultiScreenSize.minX, m_MultiScreenSize.minY);

                if (snipper.ShowDialog() == DialogResult.OK) {
                    rect = snipper.rcSelect;
                    return snipper.Image;
                }
            }

            rect = Rectangle.Empty;
            return null;
        }


        private static Size BitmapSize;
        private static Graphics Graph;
        private Point pntStart;
        public Rectangle rcSelect;

        /// <summary>
        /// Wether you want to use it on specific size and set <see cref="Form.Location"/> after displaying or go full screen.
        /// </summary>
        public bool SpecificWindowMode { get; set; } //default false

        public SnippingTool(Image screenShot) {
            InitializeComponent();
            this.BackgroundImage = screenShot;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.DoubleBuffered = true;
            this.TopMost = true;
        }

        /// <summary>
        /// The snipped picture.
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// The rectangle of the image inside of the window or screen.
        /// </summary>
        public Rectangle ImageRectangle { get; private set; }
        private static MultiScreenSize FindMultiScreenSize() {
            int minX = Screen.AllScreens[0].Bounds.X;
            int minY = Screen.AllScreens[0].Bounds.Y;

            int maxRight = Screen.AllScreens[0].Bounds.Right;
            int maxBottom = Screen.AllScreens[0].Bounds.Bottom;

            foreach (Screen aScreen in Screen.AllScreens) {
                if (aScreen.Bounds.X < minX) {
                    minX = aScreen.Bounds.X;
                }

                if (aScreen.Bounds.Y < minY) {
                    minY = aScreen.Bounds.Y;
                }

                if (aScreen.Bounds.Right > maxRight) {
                    maxRight = aScreen.Bounds.Right;
                }

                if (aScreen.Bounds.Bottom > maxBottom) {
                    maxBottom = aScreen.Bounds.Bottom;
                }
            }
            return new MultiScreenSize {minX = minX, minY = minY, maxBottom = maxBottom, maxRight = maxRight};
        }


        protected override void OnMouseDown(MouseEventArgs e) {
            // Start the snip on mouse down'
            if (e.Button != MouseButtons.Left) {
                return;
            }
            pntStart = e.Location;
            rcSelect = new Rectangle(e.Location, new Size(0, 0));
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            // Modify the selection on mouse move'
            if (e.Button != MouseButtons.Left) {
                return;
            }
            int x1 = Math.Min(e.X, pntStart.X);
            int y1 = Math.Min(e.Y, pntStart.Y);
            int x2 = Math.Max(e.X, pntStart.X);
            int y2 = Math.Max(e.Y, pntStart.Y);
            rcSelect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            this.Invalidate();
        }


        protected override void OnMouseUp(MouseEventArgs e) {
            // Complete the snip on mouse-up'
            if (rcSelect.Width <= 0 || rcSelect.Height <= 0) {
                return;
            }
            Image = new Bitmap(rcSelect.Width, rcSelect.Height);
            using (Graphics gr = Graphics.FromImage(Image)) {
                gr.DrawImage(this.BackgroundImage, new Rectangle(0, 0, Image.Width, Image.Height), rcSelect, GraphicsUnit.Pixel);
            }
            DialogResult = DialogResult.OK;
            if (mhook != null)
                mhook.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e) {
            // Draw the current selection'
            using (Brush br = new SolidBrush(Color.FromArgb(70, Color.White))) {
                int x1 = rcSelect.X;
                int x2 = rcSelect.X + rcSelect.Width;
                int y1 = rcSelect.Y;
                int y2 = rcSelect.Y + rcSelect.Height;
                e.Graphics.FillRectangle(br, new Rectangle(0, 0, x1, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x2, 0, this.Width - x2, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x1, 0, x2 - x1, y1));
                e.Graphics.FillRectangle(br, new Rectangle(x1, y2, x2 - x1, this.Height - y2));
            }
            using (var pen = new Pen(Color.Red, 1)) {
                e.Graphics.DrawRectangle(pen, rcSelect);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            // Allow canceling the snip with the Escape key'
            if (keyData == Keys.Escape) {
                this.DialogResult = DialogResult.Cancel;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private MouseListener mhook;
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            if (SpecificWindowMode) {
                this.Size = BackgroundImage.Size;
                mhook = new MouseListener();
                
                mhook.Click += (sender, loc) => {
                                    if (loc.X > Location.X && loc.X < Location.X + Size.Width && loc.Y > Location.Y && loc.Y < Location.Y + Size.Height)
                                        return;
                                    DialogResult = DialogResult.Abort;
                                    SystemSounds.Beep.Play();
                                   
                                    this.Close();
                                    mhook.Stop();
                               };
                mhook.Start();



                return;
            }
            //full screen mode:
            MultiScreenSize m_MultiScreenSize = FindMultiScreenSize();
            this.Size = new Size(m_MultiScreenSize.maxRight - m_MultiScreenSize.minX, m_MultiScreenSize.maxBottom - m_MultiScreenSize.minY);

            Graph.CopyFromScreen(m_MultiScreenSize.minX, m_MultiScreenSize.minY, 0, 0, BitmapSize);
        }

        private struct MultiScreenSize {
            public int maxBottom;
            public int maxRight;
            public int minX;
            public int minY;
        }
    }

    //=======================================================
    //Service provided by Telerik (www.telerik.com)
    //Conversion powered by NRefactory.
    //Twitter: @telerik
    //Facebook: facebook.com/telerik
    //=======================================================
}*/