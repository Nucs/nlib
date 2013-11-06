using System;
using System.Threading;

public class ValueWatcher<TValue> : IDisposable {
    private readonly ManualResetEvent _ev = new ManualResetEvent(false);
    private readonly Func<TValue, bool> _isValueAcceptableFunc;

    public ValueWatcher(Func<TValue> CurrentValueFunc, Func<TValue, bool> IsValueAcceptableFunc) {
        _isValueAcceptableFunc = IsValueAcceptableFunc;
        ValueUpdated(CurrentValueFunc.Invoke());
    }

    #region IDisposable Members

    public void Dispose() {
        Dispose(true);
    }

    #endregion

    public void ValueUpdated(TValue Value) {
        if (_isValueAcceptableFunc.Invoke(Value))
            _ev.Set();
        else
            _ev.Reset();
    }

    public bool Wait() {
        return _ev.WaitOne();
    }

    public bool Wait(int TimeoutMs) {
        return _ev.WaitOne(TimeoutMs);
    }

    public bool Wait(TimeSpan ts) {
        return _ev.WaitOne(ts);
    }

    private void Dispose(bool Disposing) {
        if (Disposing)
            _ev.Dispose();
    }
}