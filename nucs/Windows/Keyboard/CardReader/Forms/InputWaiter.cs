using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;
using nucs.Utils;

namespace nucs.Windows.Keyboard.CardReader.Forms {
    public partial class InputWaiter : Form {
        private readonly InputDevice id;
        public string Output = String.Empty;
        public CRManager.Device CR;
        private string InsertedCRsID { get; set; }
        public InputWaiter(string CRsID) {
            if (!CRManager.IsConnected(CRsID)) {
                if (MessageBox.Show("נראה שהקורא כרטיסים אינו מחובר, האם ברצונך להמשיך בכל זאת?", "שגיאה",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Error)==DialogResult.No) {

                    Dispose();
                    return;
                }
            }

            this.InsertedCRsID = CRsID;
            InitializeComponent();
            id = new InputDevice(Handle);
            id.KeyPressed += new InputDevice.DeviceEventHandler(m_KeyPressed);
        }

        private bool skip = false;
        private void m_KeyPressed(object sender, InputDevice.KeyControlEventArgs e) {
            if (Tools.ApplicationIsActivated(Process.GetCurrentProcess().Id) && e.Keyboard.deviceName == InsertedCRsID && CRManager.IsConnected(InsertedCRsID)) {
                if (skip) {
                    Console.WriteLine(e.Keyboard.vKey + ":" + e.Keyboard.key);
                    if (e.Keyboard.vKey == Keys.Enter.ToString()) {
                        SystemSounds.Beep.Play();
                        id.KeyPressed -= m_KeyPressed;
                        Close();
                    }
                }
                skip = !skip;
            } else {
                Output = String.Empty;
                    skip = false;
                }
        }

        private void InputWait_FormClosing(object sender, FormClosingEventArgs e) {
            id.Dispose();
        }

        protected override void WndProc(ref Message message) {
            if (id != null) {
                id.ProcessMessage(message);
            }
            base.WndProc(ref message);
        }

        private void InputWaiter_Load(object sender, EventArgs e) {

        }

        private void InputWaiter_KeyPress(object sender, KeyPressEventArgs e) {
            Output += e.KeyChar;
        }

        /// <summary>Returns true if the current application has focus, false otherwise</summary>

    }
}
