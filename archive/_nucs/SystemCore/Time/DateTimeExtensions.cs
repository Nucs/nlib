using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using nucs.Windows.Keyboard.CardReader.Forms;
using Timer = System.Windows.Forms.Timer;

namespace nucs.Utils {
    public static class DateTimeExtensions {
        public const string _MinDate = "2000-01-01 00:00:00";

        public static CultureInfo JewishCulture = CultureInfo.CreateSpecificCulture("he-IL");
        public static Calendar JewishCalendar = JewishCulture.DateTimeFormat.Calendar = new HebrewCalendar();

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

        /// <summary>
        ///     מאמת מס תעודת זהות, האם הוא הגיוני.
        /// </summary>
        /// <param name="IDNum"></param>
        /// <returns></returns>
        public static bool ValidateID(string IDNum) {
            if (IDNum.Length < 9)
                while (IDNum.Length < 9)
                    IDNum = '0' + IDNum;

            var mone = 0;
            for (var i = 0; i < 9; i++) {
                var incNum = Convert.ToInt32(IDNum[i].ToString());
                incNum *= (i % 2) + 1;
                if (incNum > 9)
                    incNum -= 9;
                mone += incNum;
            }
            return mone % 10 == 0;
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
            if ((a + b).Contains("?"))
                return str;
            return a + b;
        }

        public static bool AllLetters(string str) {
            return !string.IsNullOrEmpty(str) && str.All(char.IsLetter);
        }

        public static TEnum ToEnum<TEnum>(string strEnumValue, TEnum defaultValue) {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
                return defaultValue;

            return (TEnum) Enum.Parse(typeof(TEnum), strEnumValue);
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
            return (UnmanagedMemoryStream) stream;
        }

        public static string ToTimeStamp(DateTime time) {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void validateOnlyLettersEvent(object sender, KeyPressEventArgs e) {
            if (!char.IsLetter(e.KeyChar) && (Keys) e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        public static void validateOnlyDigitsEvent(object sender, KeyPressEventArgs e) {
            if (!char.IsDigit(e.KeyChar) && (Keys) e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        public static void validateOnlyDigitsAndLettersEvent(object sender, KeyPressEventArgs e) {
            if (!char.IsLetterOrDigit(e.KeyChar) && (Keys) e.KeyChar != Keys.Back) {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        public static bool IsEnglish(string str) {
            return "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".Any(str.Contains);
        }

        /// <summary>
        ///     Uses form InputWaiter.cs inorder to get the cardcode that is received from reader.
        ///     returns: string - completed: the code, form closed: String.Empty
        /// </summary>
        /// <returns></returns>
        public static string GetCardCode(string CRsID) {
/*            if (!CRManager.IsConnected(Settings.Default.CRsID)) {
                return String.Empty;
            }*/
            var dialog = new InputWaiter(CRsID);
            if (dialog.IsDisposed)
                return string.Empty;
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
            if (activatedHandle == IntPtr.Zero)
                return false; // No window is currently activated

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

        public static string DateTimeNowHebrew() {
            return ParseHebrewDateTimeString(DateTime.Now.ToString(JewishCulture)).ToString(JewishCulture);
        }

        public static string DateNowHebrew() {
            var s = ParseHebrewDateTimeString(DateTime.Now.Date.ToString(JewishCulture)).ToString(JewishCulture).Split(' ');
            return string.Format("{0} {1} {2}", s[0], s[1], s[2]);
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

    internal class DualImages {
        public DualImages(int n, int milliseconds, Image img, Image blink, Timer timer, ToolStripMenuItem destinition) {
            this.img = img;
            this.blink = blink;
            this.milliseconds = milliseconds;
            this.timer = timer;
            times = n;
            blinked = false;
            this.destinition = destinition;
        }

        public Image img { get; set; }
        public Image blink { get; set; }
        public int times { get; set; }
        public int milliseconds { get; set; }
        public Timer timer { get; set; }
        public bool blinked { get; set; }
        public ToolStripMenuItem destinition { get; set; }
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