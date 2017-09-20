using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace nucs.Toaster {
    public partial class ToastsManager {
        private const byte MaxNotifications = 4;
        private readonly ToastsCollection _buffer = new ToastsCollection();
        private int _count;
        public ToastsCollection ToastsCollection = new ToastsCollection();

        public ToastsManager(NotificationLocation location = NotificationLocation.BottonRight) {
            InitializeComponent();
            NotificationsControl.DataContext = ToastsCollection;
            SetNotificationsLocation(location);

        }

        internal ToastsManager(ManualResetEventSlim reset, NotificationLocation location = NotificationLocation.BottonRight) : this(location) {
            reset.Set();
        }
        
        public void SetNotificationsLocation(NotificationLocation location) {
            if (!Dispatcher.CheckAccess()) { // CheckAccess returns true if you're on the dispatcher thread
                Dispatcher.Invoke(new Action(() => SetNotificationsLocation(location)));
                return;
            }
            switch (location) {
                case NotificationLocation.TopRight:
                    Top = 0;
                    Left = SystemParameters.PrimaryScreenWidth - Width;
                    NotificationsControl.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotificationLocation.BottonRight:
                    Top = SystemParameters.PrimaryScreenHeight - Height;
                    Left = SystemParameters.PrimaryScreenWidth - Width;
                    NotificationsControl.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case NotificationLocation.TopLeft:
                    Top = 0;
                    Left = 0;
                    NotificationsControl.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotificationLocation.ButtomLeft:
                    Top = SystemParameters.PrimaryScreenHeight - Height;
                    Left = 0;
                    NotificationsControl.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        public void AddNotification(Toast toast) {
            if (!Dispatcher.CheckAccess()) { // CheckAccess returns true if you're on the dispatcher thread
                Dispatcher.Invoke(new Action(() => AddNotification(toast)));
                return;
            }

            toast.Id = _count++;
            if (ToastsCollection.Count + 1 > MaxNotifications)
                _buffer.Add(toast);
            else
                ToastsCollection.Add(toast);

            //Show window if there're notifications
            if (ToastsCollection.Count > 0 && !IsActive)
                Show();
        }

        public void RemoveNotification(Toast toast) {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Action(() => RemoveNotification(toast)));
                return;
            }
            if (ToastsCollection.Contains(toast))
                ToastsCollection.Remove(toast);

            if (_buffer.Count > 0) {
                ToastsCollection.Add(_buffer[0]);
                _buffer.RemoveAt(0);
            }

            //Close window if there's nothing to show
            if (ToastsCollection.Count < 1)
                Hide();
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e) {
            if (Math.Abs(e.NewSize.Height) > 1)
                return;
            var element = sender as Grid;
            RemoveNotification(ToastsCollection.First(n => n.Id == int.Parse(element.Tag.ToString())));
        }
    }

    public enum NotificationLocation {
        TopRight,
        BottonRight,
        TopLeft,
        ButtomLeft
    }
}