#if !AV_SAFE
#if !(NET35 || NET3 || NET2)

using System;
using System.Diagnostics;
using System.Text;
#if NET4_5
using System.Threading;
using System.Threading.Tasks;
#else
using nucs.Mono.System.Threading;
#endif
using nucs.Windows.Processes;

namespace nucs.Windows.Keyboard {
    public delegate void OnCancelHandler(DateTime time);
    public delegate void OnTickHandler(int tickNum);
    public delegate void OnMessageSentHandler(DateTime time, StringBuilder NextMessage);

    public sealed class TypingTarget : IDisposable {
        /// <summary>
        /// For e.g. every 6 seconds, a message is sent, so 6 / 3 is 2 seconds, and so every 2 seconds there will be a tick
        /// the amount of ticks can be changed in property Ticks (default is 3)
        /// </summary>
        public OnTickHandler OnTick;

        /// <summary>
        /// Invoked at every message sending.
        /// </summary>
        public OnMessageSentHandler OnMessageSent;

        /// <summary>
        /// For e.g. every 6 seconds, a message is sent, so 6 / 3 is 2 seconds, and so every 2 seconds there will be a tick
        /// the amount of ticks can be changed in property Ticks (default is 3). also this is 1 based.
        /// </summary>
        public uint Ticks { get { return _ticks; } set { _ticks = value; if (value == 0) _ticks = 1; } }
        private uint _ticks = 3;

        public ProcessInfo ProcessBeingFocused { get; set; }
/*        private CancelToken _cancelToken = null;
        public CancelToken CancelToken {
            get {
                if (_cancelToken == null || _cancelToken.Cancelled)
                    return null;
                return _cancelToken;
            }
            private set { if (_cancelToken != null && !_cancelToken.Cancelled) return; _cancelToken = value; }
        }*/

        public bool IsRunning { get { return (typer == null); } }
#if NET4_5
        private Task typer;
#else
        private VoidTask typer;
#endif
        public bool Disposed {get { return _disposed; } }
        private bool _disposed = false;

        public TypingTarget() {
            OnTick += num => { };
            OnMessageSent += (time, dd) => { };
            ProcessBeingFocused = null;
        }

        public TypingTarget(ProcessInfo process) : this() {
            ProcessBeingFocused = process;
        }

        public TypingTarget(uint ticks) : this() {
            _ticks = ticks;
        }

        public TypingTarget(ProcessInfo process, uint ticks) : this(process) {
            _ticks = ticks;
        }

        /// <summary>
        /// Sends a message, regardless to the auto type and it's settings..
        /// </summary>
        /// <param name="text"></param>
        /// <param name="OnlyInSpecificProcess"> </param>
        /// <returns>Succeeded sending the message to the specific process. if OnlyInSpecificProcess = false, returns true always</returns>
        public bool SendMessage(string text, bool OnlyInSpecificProcess = false) {
            if (Disposed || (OnlyInSpecificProcess == true && ProcessBeingFocused == null ))
                return false;
            var FocusedProc = ProcessFinder.GetForegroundProcess();
            if (Process.GetCurrentProcess().Id == FocusedProc.Id)
                return false;
            if (!OnlyInSpecificProcess || FocusedProc.Id == ProcessBeingFocused.UniqueID) {
                KeySender.SendText(text);
                KeySender.PressKey(KeyCode.Return);
                return true;
            }
            return false;
        }

        private CancellationTokenSource CancelToken;

#if (NET4_5_1 || NET4_5)
        public async Task<bool> StartSending(string text, uint delay) {
            return await Task.Run(async () => {
#else
        public Task<bool> StartSending(string text, uint delay) {
            return Task.Run(() => {
#endif
                delay = (uint) (delay - delay*0.08);
                if (Disposed) return false;
                if (CancelToken != null && CancelToken.IsCancellationRequested && typer != null)
#if NET4_5
                    await typer;
#else
                    typer.Wait();
#endif
                uint tickPeroid = delay;
                if (_ticks != 0)
                    tickPeroid = (uint) Convert.ToInt32(delay/_ticks);
                CancelToken = new CancellationTokenSource();
                var token = CancelToken.Token;
                typer = Task.Run(() => {
                                        var builder = new StringBuilder(text);
                                        var sw = new Stopwatch();
                                        while (true) {
                                            try {
                                                var slimTickPeroid = tickPeroid/8;
                                                sw.Restart();
                                                for (var i = (_ticks == 0) ? -1 : 1; i <= _ticks; i++) {

                                                    for (var j = 0; j < 8; j++) {
                                                        token.ThrowIfCancellationRequested();
                                                        Task.Delay((int) slimTickPeroid);
                                                    }
                                                    if (_ticks != 0)
                                                        OnTick(i);
                                                    if (i == _ticks) {
                                                        Task.Run(() => SendMessage(builder.ToString(), ProcessBeingFocused != null));
                                                        OnMessageSent(DateTime.Now, builder);
                                                    }
                                                }

                                                Debug.Print(sw.ElapsedMilliseconds.ToString());

                                            } catch {
                                                break;
                                            }
                                        }
                                        typer = null;
                                    }, token);
                return true;
            });
        }
        #if NET4_5
        public async System.Threading.Tasks.Task<bool> StopSending() {
#else
        public bool StopSending() {
#endif
            if (Disposed || CancelToken == null ||CancelToken.IsCancellationRequested)
                return false;
#if NET4_5
            await typer;
#else
            typer.Wait();
#endif
            CancelToken.Cancel();
            return true;
        }
        
        public void Dispose() {
            try {
                if (CancelToken != null)
                    CancelToken.Cancel();
            } catch {}
            _disposed = true;
        }
    }

    
    /*public class CancelToken {
        public bool IsCancelRequested { get { return _isCancelRequested ?? false; } private set { _isCancelRequested = value; } }
        private bool? _isCancelRequested = null;

        public bool Cancelled { get { return cancelled; } }
        private bool cancelled;
        public OnCancelHandler OnCancel;

        #region Inits
        /// <param name="state">true, is for cancelled, false for not</param>
        public CancelToken(bool state) {
            OnCancel += time => { };
            cancelled = state;
        }
        /// <param name="state">true, is for cancelled, false for not</param>
        public CancelToken(bool state, params OnCancelHandler[] actions) : this(state) {
            foreach (var a in actions)
                OnCancel += a;
        }
        #endregion

        public bool ApproveCancel() {
            if ((IsCancelRequested) == false) return false;
            cancelled = true;
            OnCancel(DateTime.Now);
            return true;
        }

        public bool RequestCancel() {
            if (cancelled)
                return false;
            _isCancelRequested = true;
            return true;

        }

    }*/
}
#endif

#endif