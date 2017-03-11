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
        /*        /// <summary>
                ///     When the toast has been clicked
                /// </summary>
                public event Action<Toast> Clicked;

                /// <summary>
                ///     When the toast has been closed.
                /// </summary>
                public event Action<Toast> Closed;*/
        /*
                public ICommand ClickedCommand { get; set; }
                public ICommand ClosedCommand { get; set; }*/
        public ICommand Command { get; set; }

        //todo toaster on exit - invoke and so on.
        private int _id;

        private BitmapImage _image;

        private string _message;

        private string _subTitle;

        private string _title;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Toast() {
/*            ClickedCommand = new DynCommand(o => Clicked?.Invoke(this));
            ClosedCommand = new DynCommand(o => Closed?.Invoke(this));*/
        }

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


        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        public object Clone() {
            return new Toast {Image = Image, BackgroundColor = BackgroundColor,/* ClickedCommand = ClickedCommand,*/ TextColor = TextColor, SubtitleColor = SubtitleColor, TitleColor = TitleColor};
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    

    internal class DynCommand : ICommand {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public DynCommand(Action<object> command) {
            Command = command;
        }
        public Action<object> Command { get; }
        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter) {
            Command?.Invoke(parameter);
        }

        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }

}