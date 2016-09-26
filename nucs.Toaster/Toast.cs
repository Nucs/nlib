using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;

namespace nucs.Toaster {
    /// <summary>
    ///     A single toast notification
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Toast : INotifyPropertyChanged, ICloneable {
        private int _id;

        private BitmapImage _image;
        private string _message;

        private string _subTitle;

        private string _title;


        public string Message {
            get { return _message; }

            set {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public int Id {
            get { return _id; }

            set {
                if (_id == value)
                    return;
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public BitmapImage Image {
            get { return _image; }

            set {
                if (Equals(_image, value))
                    return;
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        public string Title {
            get { return _title; }

            set {
                if (_title == value)
                    return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string SubTitle {
            get { return _subTitle; }

            set {
                if (_subTitle == value)
                    return;
                _subTitle = value;
                OnPropertyChanged("SubTitle");
            }
        }

        public Color BackgroundColor { get; set; } = Color.FromRgb(0x2a, 0x33, 0x45);

        public Color TitleColor { get; set; } = Colors.White;

        public Color SubtitleColor { get; set; } = Colors.CornflowerBlue;

        public Color TextColor { get; set; } = Colors.White;

        public ICommand Command { get; set; }

        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        public object Clone() {
            return new Toast {Image = Image, BackgroundColor = BackgroundColor, Command = Command, TextColor = TextColor, SubtitleColor = SubtitleColor, TitleColor = TitleColor};
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}