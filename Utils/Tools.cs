using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using nucs.ADO.NET;
using nucs.ADO.NET.MySql;
using nucs.Windows.Keyboard.CardReader.Forms;
using Thread = System.Threading.Thread;
using Timer = System.Windows.Forms.Timer;

namespace nucs.Utils {
    public static class Tools {

        public const string _MinDate = "2000-01-01 00:00:00";
        /// <summary>
        ///     Cleans the null slots from the array and returns clean minimized one.
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="array"> the array to clean</param>
        /// <returns></returns>
        public static T[] MinimizeArray<T>(T[] array) {
            return array.Where(t => t != null).ToArray();
        }

        public static void CleanArraysNulls<T>(ref T[] array) {
            array = array.Where(t => t != null).ToArray();
        }

        private static void CAATThread(int milliseconds, Action action) {
            Thread.Sleep(milliseconds);
            action.Invoke();
        }

        public static void CallActionAfterTime(int milliseconds, Action action) {
            var thread = new Thread(() => CAATThread(milliseconds, action));
            thread.Start();
        }

        public static bool ValidateID(string IDNum) {
            if (IDNum.Length < 9) {
                while (IDNum.Length < 9) {
                    IDNum = '0' + IDNum;
                }
            }

            var mone = 0;
            for (var i = 0; i < 9; i++) {
                var incNum = Convert.ToInt32(IDNum[i].ToString());
                incNum *= (i%2) + 1;
                if (incNum > 9)
                    incNum -= 9;
                mone += incNum;
            }
            return mone%10 == 0;
        }

        public static bool ValidateID(long id) {
            return id > 99999999 && ValidateID(id.ToString());
        }

        public static void autoHidePicture(PictureBox pic, int milliseconds) {
            pic.Show();
            var timer = new Timer {Tag = pic, Interval = milliseconds, Enabled = true};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public static void timer_Tick(object sender, EventArgs e) {
            var timer = sender as Timer;
            var pic = timer.Tag as PictureBox;
            timer.Enabled = false;
            pic.Hide();
            timer.Dispose();
        }

        public static string FirstLetterToUpper(string str) {
            if (!string.IsNullOrEmpty(str) && IsEnglish(str[0].ToString())) {
                var a = (str.ToCharArray()[0] = str.Substring(0).ToUpper().ToCharArray()[0]);
                return a.ToString();
            }

            return str;


        }

        public static string FirstLetterToUpperRestToLower(string str) {
            if (!IsEnglish(str))
                return str;
            var a = str.Substring(0, 1).ToUpper();
            var b = str.Substring(1, str.Length - 1).ToLower();
            Console.WriteLine(a+b);
            if ((a + b).Contains("?"))
                return str;
            return a + b;
        }

        public static bool AllLetters(string str) {
            return !string.IsNullOrEmpty(str) && str.All(char.IsLetter);
        }

        public static TEnum ToEnum<TEnum>(string strEnumValue, TEnum defaultValue) {
            if (!Enum.IsDefined(typeof (TEnum), strEnumValue))
                return defaultValue;

            return (TEnum) Enum.Parse(typeof (TEnum), strEnumValue);
        }


/*        public enum ConnectionSetting {
            Hostname=0,
            Database=1,
            UserId=2,
            Password=3
        }*/

        public static UnmanagedMemoryStream GetResourceStream(string resName) {
            var assembly = Assembly.GetExecutingAssembly();
            var strResources = assembly.GetName().Name + ".g.resources";
            var rStream = assembly.GetManifestResourceStream(strResources);
            var resourceReader = new ResourceReader(rStream);
            var items = resourceReader.OfType<DictionaryEntry>();
            var stream = items.First(x => (x.Key as string) == resName.ToLower()).Value;
            return (UnmanagedMemoryStream)stream;
        }

        public static string ToTimeStamp(DateTime time) {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
            //'2038-01-19 03:14:07
/*            var timeresult = "";
            var s = time.ToString().Split(' ');
            if (s[2] == "PM") {
                var ss = s[1].Split(':');
                timeresult = Convert.ToInt32(ss[0])+12 + ":" + ss[1] + ":" + ss[2];
            } else {
                timeresult = s[1];
            }
            return s[0].Replace("/", "-")+" "+timeresult;*/
        }


        public static DateTime ToDateTime(string timeStamp) {
            return new MySqlDateTime(timeStamp).GetDateTime();
        }

        public static void FixUnlistedStudentsInvalidDate(string connQuery) {
            // 01/01/0001 12:00:00 AM
            using (var conn = new MySqlConnectionAuto(connQuery))
                using (var comm = new MySqlCommand("SELECT Id, Date FROM unlistedstudents;", conn.Connection))
                    using (var reader = comm.ExecuteReader()) {
                        while (reader.Read()) {
                            try {
                                if ((new MySqlDateTime(Convert.ToDateTime(reader["Date"])).GetDateTime().ToString("dd-MM-yyyy hh:mm")) == "01-01-0001 12:00")
                                    throw new FormatException();
                            } catch (FormatException) {
                                FixDate(connQuery, (int)reader["Id"]);
                            }
                        }
                    }
        }

        private static void FixDate(string connQuery, int Id) {
            using (var conn = new MySqlConnectionAuto(connQuery))
                using (var comm = new MySqlCommand("", conn.Connection)) {
                    comm.CommandText = "UPDATE unlistedstudents SET Date='" + _MinDate + "' WHERE Id=" + Id + ";";
                    comm.ExecuteNonQuery();
                }
        }

        public static void validateOnlyLettersEvent(object sender, KeyPressEventArgs e) {
            if (!char.IsLetter(e.KeyChar) && (Keys)e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        public static void validateOnlyDigitsEvent(object sender, KeyPressEventArgs e) {
            if (!char.IsDigit(e.KeyChar) && (Keys)e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        public static void validateOnlyDigitsAndLettersEvent(object sender, KeyPressEventArgs e) {
            if (!char.IsLetterOrDigit(e.KeyChar) && (Keys)e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        public static bool IsEnglish(string str) {
            return "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".Any(str.Contains);
        }
        /// <summary>
        /// Uses form InputWaiter.cs inorder to get the cardcode that is received from reader.
        /// returns: string - completed: the code, form closed: String.Empty
        /// </summary>
        /// <returns></returns>
        public static string GetCardCode(string CRsID) {
/*            if (!CRManager.IsConnected(Settings.Default.CRsID)) {
                return String.Empty;
            }*/
            var dialog = new InputWaiter(CRsID);
            if (dialog.IsDisposed) return String.Empty;
            dialog.ShowDialog();
            var result = dialog.Output;
            dialog.Dispose();
            return result;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        //
        public static bool ApplicationIsActivated() {
            return ApplicationIsActivated(Process.GetCurrentProcess().Id);
        }

        public static bool ApplicationIsActivated(int ProcID) {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero) {
                return false;       // No window is currently activated
            }

            var procId = ProcID;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        public static Form GetTopMostForm() {
            try {
                return Application.OpenForms
                .Cast<Form>()
                .First(x => x.Focused);
            } catch {
                return null;
            }

        }

        public static CultureInfo JewishCulture = CultureInfo.CreateSpecificCulture("he-IL");
        public static Calendar JewishCalendar = JewishCulture.DateTimeFormat.Calendar = new HebrewCalendar();
        public static string DateTimeNowHebrew() {
            return ParseHebrewDateTimeString(DateTime.Now.ToString(JewishCulture)).ToString(JewishCulture);
        }
        public static string DateNowHebrew() {
            var s = ParseHebrewDateTimeString(DateTime.Now.Date.ToString(JewishCulture)).ToString(JewishCulture).Split(' ');
            return string.Format("{0} {1} {2}",s[0] , s[1] , s[2]);
        }

        public static DateTime ParseHebrewDateTimeString(string strHebrew) {
            return DateTime.Parse(strHebrew, JewishCulture);
        }

/*        public static readonly List<Action> waitingForConnList = new List<Action>();
        [DebuggerStepThrough]
        public static void InvokeConnectionRequired(Action trigger) {
            if (Connected)
                trigger();
            else
                waitingForConnList.Add(trigger);
        }*/

    }
        class DualImages {
        public Image img {get; set; }
        public Image blink {get; set; }
        public int times { get; set; }
        public int milliseconds { get; set; }
        public Timer timer { get; set; }
        public bool blinked { get; set; }
        public ToolStripMenuItem destinition { get; set; }
        public DualImages(int n, int milliseconds, Image img, Image blink, Timer timer, ToolStripMenuItem destinition) {
            this.img = img;
            this.blink = blink;
            this.milliseconds = milliseconds;
            this.timer = timer;
            times = n;
            blinked = false;
            this.destinition = destinition;
        }
    }
}

/*        public static bool ValidateRealID(string id) {
                if (id.Length!=9)
                    return false;
                int[] id_12_digits = {1,2,1,2,1,2,1,2,1};
                int count = 0;

                id = id.PadLeft(9, '0');

                for (int i = 0; i < 9; i++)
                {
                    int num = Int32.Parse(id.Substring(i, 1)) * id_12_digits[i];

                if (num > 9)
                num = (num / 10) + (num % 10);

                count += num;
                }
                return (count % 10 == 0);

            }

            public static bool ValidateRealID(long id) {
                if (id > 100000000)
                    return false;
                return ValidateRealID(id.ToString());

            }*/


