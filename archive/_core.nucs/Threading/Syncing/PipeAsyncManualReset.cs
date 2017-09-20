// Decompiled with JetBrains decompiler
// Type: nucs.Threading.Syncers.Pipes.PipeAsyncManualReset`1
// Assembly: nucs.Threading.Syncers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A7513D5A-CDD3-4CF6-8FCA-4D412887F2A3
// Assembly location: D:\C#\CodeMusic\CodeMusic.UrlSearcher\CodeMusic.UrlSearcher\bin\Debug\nucs.Threading.Syncers.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Concurrent.Pipes;
using Nito.AsyncEx;

namespace nucs.Threading.Syncing {
    public class PipeAsyncManualReset<T> : Pipe<T>, IWaitable {
        private readonly AsyncManualResetEvent _reset = new AsyncManualResetEvent(false);

        public PipeAsyncManualReset() {}

        public PipeAsyncManualReset(IPipe<T> receiveFrom) {
            ConnectFrom(receiveFrom);
        }

        public bool IsSet {
            get { return _reset.IsSet; }
        }

        public Task WaitAsync() {
            return _reset.WaitAsync();
        }

        public Task<bool> WaitAsync(int millisecondsTimeout) {
            return _reset.WaitAsync(millisecondsTimeout);
        }

        public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken) {
            return _reset.WaitAsync(millisecondsTimeout, cancellationToken);
        }

        public Task<bool> WaitAsync(TimeSpan timespan) {
            return _reset.WaitAsync(timespan);
        }

        public Task<bool> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken) {
            return _reset.WaitAsync(timespan, cancellationToken);
        }

        public Task WaitAsync(CancellationToken cancellationToken) {
            return _reset.WaitAsync(cancellationToken);
        }

        public void Wait() {
            _reset.Wait();
        }

        public bool Wait(int milliseconds) {
            return _reset.Wait(milliseconds);
        }

        public bool Wait(TimeSpan timespan) {
            return _reset.Wait(timespan);
        }

        public void Wait(CancellationToken cancellationToken) {
            _reset.Wait(cancellationToken);
        }

        public event Action<T> Incoming;

        public override void IncomingItem(T o) {
            base.IncomingItem(o);
            _reset.Set();
            // ISSUE: reference to a compiler-generated field
            var incoming = Incoming;
            if (incoming == null)
                return;
            var obj = o;
            incoming(obj);
        }

        public void Reset() {
            _reset.Reset();
        }
    }
}