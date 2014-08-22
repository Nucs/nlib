using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

using System.Windows.Forms;
using nucs.Network;
using nucs.SystemCore.String;
using nucs.Utils;
#if NET_4_5
using System.Threading;
using System.Threading.Tasks;
#else
using System.Threading;
using nucs.Mono.System.Threading;
#endif

namespace nucs.Forms {

    /// <summary>
    /// A form that will request ip address from user and a port, use static method for this.
    /// </summary>
    public partial class UserConnectionDetailsRequester : Form {
        private bool ForceTesting { get; set; } //default: false
        private bool PlannedClosing { get; set; } //default: false
        internal bool Cancelled { get; set; } //default: false
        internal UserConnectionDetailsRequester( bool forceTesting = true, ConnectionDetails default_details = null) {
            InitializeComponent();
            this.FormClosing += (sender, args) => { if (PlannedClosing == false) Cancelled = true; };
            ForceTesting = forceTesting;
            if (default_details == null) return;
            txtIP.Text = default_details.IP;
            txtPort.Text = default_details.Port.ToString(CultureInfo.InvariantCulture);
            if (default_details.DefaultPort)
                txtPort.Enabled = false;
            if (default_details.DefaultIP)
                txtIP.Enabled = false;
        }

        internal ConnectionDetails Results() {
            return new ConnectionDetails(_ip, _port, !txtPort.Enabled, !txtIP.Enabled) {Cancelled = Cancelled};
        }

        /// <summary>
        /// Will open a form that will ask for connection details (with option for default props) and attempt to ping target to see if it is possible to establish connection
        /// </summary>
        /// <param name="parent">Parent form that this form will be opened on</param>
        /// <param name="default_details">The default connection details to be inserted</param>
        /// <param name="forceTesting"></param>
        public static ConnectionDetails GetDetails(Form parent, ConnectionDetails default_details, bool forceTesting = true) {
            UserConnectionDetailsRequester getter = null;
            parent.Invoke(() => {
                              getter = new UserConnectionDetailsRequester(forceTesting, default_details);
                              getter.ShowDialog(parent);
                          });
            return getter.Results();
        }


        private void txtPort_KeyPress(object sender, KeyPressEventArgs e) { //allows only digits to be typed and backspace.
            if (!char.IsDigit(e.KeyChar) && (Keys)e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        private string _ip;
        private int _port;
        private void btnComplete_Click(object sender, EventArgs e) {
            if (txtIP.Enabled == false && txtPort.Enabled == false)
                goto _successful;

            if (string.IsNullOrEmpty(txtIP.Text) || string.IsNullOrEmpty(txtPort.Text)) {
                SystemSounds.Beep.Play();
                return;
            }
            bool conn_test = false;
            try {
                conn_test = InternetTools.TestHostConnection(IPAddress.Parse(txtIP.Text), 1);
            } catch (Exception) {
                MessageBoxy.Show("לא היה ניתן לאתר את הכתובת שהוזנה, נסה ללחוץ על כפתור הריענון ובחר את אחת מההצעות.\nאם אין כלל, סימן שהכתובת לא קיימת או לא זמינה.", "שגיאה",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                return;
            }

            if (conn_test == false) {
                if (ForceTesting == false) {
                    if (MessageBoxy.Show("לא היה ניתן לאתר את הכתובת שהוזנה, האם ברצונך בכל זאת להמשיך?", "תוצאות הבדיקה", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, true) == DialogResult.No) {
                        return;
                    }
                } else {
                    MessageBoxy.Show("לא היה ניתן לאתר את הכתובת שהוזנה, אנא נסה שנית או מאוחר יותר", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                }

            }
            
        _successful:
            _ip = txtIP.Text;
            _port = txtPort.Text.ToInt32();
            PlannedClosing = true;
            Hide();
            Close();
        }

        private void picRefresh_MouseDown(object sender, MouseEventArgs e) {
            picRefresh.Hide();
            var ipadd = txtIP.Text;
            Task.Run(() => {
                    var res = InternetTools.GetConnectableAliases(ipadd).Result;
                    if (res == null || res.Count == 0) {
                        picRefresh.Show();
                        return;
                    }
                    var ips = res.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Select(ip => ip.ToString()).ToArray();
                    Invoke(new MethodInvoker(() => {
                        txtIP.Text = "";
                        txtIP.Items.Clear();
                        txtIP.Items.AddRange(ips);
                        if (txtIP.Items.Count > 0)
                            txtIP.SelectedIndex = 0;
                        picRefresh.Show();
                    }));
                });
        }
    }

    public class ConnectionDetails {
        public string IP { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// Will disable the ability to change the port.
        /// </summary>
        public bool DefaultPort { get; private set; }

        /// <summary>
        /// Will disable the ability to change the IP.
        /// </summary>
        public bool DefaultIP { get; private set; }
        /// <summary>
        /// If the request was cancelled by user and no correct input is given.
        /// </summary>
        public bool Cancelled { get; set; }

        public ConnectionDetails(string ip, int port, bool DefaultPort = false, bool DefaultIP = false) {
            IP = ip;
            Port = port;
            this.DefaultPort = DefaultPort;
            this.DefaultIP = DefaultIP;
        }

        
    }
}
