using System;
using System.Media;
using System.Windows.Forms;

namespace nucs.Windows.Keyboard.CardReader.Forms {
    public partial class InputDeviceDetect : Form {

        private readonly InputDevice id;
        public CRManager.Device Output;

        public InputDeviceDetect() {
            InitializeComponent();
            id = new InputDevice(Handle);
            id.KeyPressed += new InputDevice.DeviceEventHandler(m_KeyPressed);
        }


        private int count=0;
        private int lastDevice = 0;
        private bool found = false;
        private void m_KeyPressed(object sender, InputDevice.KeyControlEventArgs e) {
            
            if ((int)e.Keyboard.deviceHandle == lastDevice)
                count++;
            else {
                count = 0;
                lastDevice = (int)e.Keyboard.deviceHandle;
            }
            if (count == 15) {
                Output = e.Keyboard;
                found = true;
            }
        }
       

        private void InputWait_FormClosing(object sender, FormClosingEventArgs e) {
            id.Dispose();
        }

        protected override void WndProc(ref Message message) {
            if (id != null) {
                //Console.WriteLine(message);
                id.ProcessMessage(message);
            }
            base.WndProc(ref message);
        }

        private void InputDeviceDetect_KeyPress(object sender, KeyPressEventArgs e) {
            if (found && (Keys)e.KeyChar == Keys.Enter) {
                SystemSounds.Beep.Play();
                id.KeyPressed -= m_KeyPressed;
                Close();
            }
        }

        private void InputDeviceDetect_Load(object sender, EventArgs e)
        {

        }

    }
}
