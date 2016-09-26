using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using nucs.Toaster.Resources;

namespace nucs.Toaster {
    public partial class App : Application {
        private readonly ManualResetEventSlim _sync;

        #region Instance Variables

        private Thread newWindowThread;

        public App(ManualResetEventSlim sync) {
            _sync = sync;
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            newWindowThread = new Thread(() => {
                _sync.Set();
                System.Windows.Threading.Dispatcher.Run();
            });
            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        protected override void OnLoadCompleted(NavigationEventArgs e) {
            base.OnLoadCompleted(e);
        }
    }

    /// <summary>
    ///     A static manager for ToastManager
    /// </summary>
    public static class Toaster {
        private static ToastsManager Manager { get; set; }
        private static App app { get; set; }
        public static Thread ToasterThread { get; private set; }
        private static readonly ManualResetEventSlim _sync = new ManualResetEventSlim(false);

        static Toaster() {
            ToasterThread = new Thread(() => {
                Manager = new ToastsManager();
                Manager.Show();

                Manager.Closed += (sender2, e2) => Manager.Dispatcher.InvokeShutdown();

                Images.Load(); //load to this dispatcher
                Default = new Toast() {
                    Title = null,
                    Image = Images.SuccessThumbs,
                    Message = null,
                    TextColor = Colors.White,
                    SubTitle = "-",
                    BackgroundColor = Colors.Black
                };

                _sync.Set();
                System.Windows.Threading.Dispatcher.Run();
            });
            ToasterThread.Name = "ToasterThread";
            ToasterThread.SetApartmentState(ApartmentState.STA);
            ToasterThread.Start();
        }

        private static readonly object sync = new object();

        public static Toast Default { get; set; }

        /// <summary>
        ///     Change the location of the notiflication
        /// </summary>
        /// <param name="location"></param>
        public static void SetNotificationLocation(NotificationLocation location) {
            Manager.SetNotificationsLocation(location);
        }

        /// <summary>
        /// Adds a toast
        /// </summary>
        /// <param name="toast"></param>
        public static void AddToast(Toast toast) {
            Manager.AddNotification(toast);
        }

        /// <summary>
        ///     Sends a toast
        /// </summary>
        public static void Add(string title, string message, BitmapImage image) {
            _sync.Wait();
            var t = Default.Clone() as Toast;
            if (image != null)
                t.Image = image;
            t.Title = title;
            t.Message = message;
            lock (sync) {
                Manager.AddNotification(t);
            }
        }

        /// <summary>
        ///     Sends a toast
        /// </summary>
        public static void Add(string title, string message, ToastImage image) {
            Add(title, message, image?.Image);
        }

        /// <summary>
        ///     Sends a toast
        /// </summary>
        public static void Add(string title, string message) {
            Add(title, message, null);
        }
    }
}