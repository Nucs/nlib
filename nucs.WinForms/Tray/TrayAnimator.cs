using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using nucs.WinForms.Tray;

namespace nucs.WinForms.Tray {
    public class TrayIconAnimator {
        private readonly Icon[] _animationFrames;
        private Task _animationTask;
        private bool _busy;
        private string _lastText;
        private readonly Icon _restStateIcon;
        private readonly nucs.WinForms.Tray.TrayIcon _trayIcon;

        public TrayIconAnimator(nucs.WinForms.Tray.TrayIcon trayIcon, Icon restStateIcon, params Icon[] animationFrames) {
            _trayIcon = trayIcon;
            _restStateIcon = restStateIcon;
            _animationFrames = animationFrames;
        }

        public void Busy(string withText = null) {
            lock (this) {
                if (_busy)
                    return;
                _busy = true;
            }
            _lastText = _trayIcon.DefaultTipText;
            _trayIcon.DefaultTipText = withText ?? "busy...";
            _animationTask = new Task(() => {
                while (_busy)
                    foreach (var frame in _animationFrames) {
                        _trayIcon.Icon = frame;
                        Thread.Sleep(200);
                    }
            });
            _animationTask.Start();
        }

        public void Rest(string withText = null) {
            lock (this) {
                if (!_busy)
                    return;
                _busy = false;
            }
            _trayIcon.DefaultTipText = withText ?? _lastText;
            _animationTask.Wait();
            _animationTask.Dispose();
            _trayIcon.Icon = _restStateIcon;
        }
    }
}