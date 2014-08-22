using System.Collections.Generic;
using System.Linq;
#if !NET_4_5
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;

namespace nucs.Mono.System.Threading {
    public delegate R TaskMethod<out R>();
    public delegate void TaskMethod();
    internal delegate R _waitress<out R>(int timeout_millsecodns, BoolHolder successful);
    internal delegate void _waitress(int timeout_millsecodns, BoolHolder successful);

    public static class Task {
        public static Task<R> Run<R>(TaskMethod<R> act) {
            return new Task<R>(act);
        }

        public static Task<R> Run<R>(TaskMethod<R> act, CancellationToken token) {
            return new Task<R>(act, token);
        }

        public static VoidTask Run(TaskMethod act) {
            return new VoidTask(act);
        }

        public static VoidTask Run(TaskMethod act, CancellationToken token) {
            return new VoidTask(act, token);
        }

        public static VoidTask Delay(int delay) {
            var tr = Run(() => Thread.Sleep(delay));
            Application.DoEvents();
            return tr;

        }
        public static Task<Task<TResult>> WhenAny<TResult>(ICollection<Task<TResult>> tasks) {

            if (tasks.Count == 0)
                throw new ArgumentException("The tasks argument contains no tasks", "tasks");
            return Run(() => tasks.AsParallel().FirstOrDefault(t => { t.Wait(); return true; }));
        }
        public static Task<Task<TResult>> WaitAll<TResult>(ICollection<Task<TResult>> tasks) {
            //todo
            throw new NotImplementedException();
        }

    }
    

    public class VoidTask : IDisposable {

        #region Properties
        public bool IsCompleted {
            get { HandleDisposion(); return _asyncTask != null && _asyncResult.IsCompleted; }
        }


        public CancellationToken CancellationToken { get { return _token; } }
        public bool IsCancelled {
            get { return _token != CancellationToken.None && _token.IsCancellationRequested; }
        }

        internal IAsyncResult _asyncResult;
        internal TaskMethod _asyncTask;

        internal _waitress _waiter;
        internal CancellationToken _token = CancellationToken.None;
        internal bool _completed = false;
        internal VoidTask(TaskMethod act) {
            BeginTask(_asyncTask = (act));
        }

        internal VoidTask(TaskMethod act, CancellationToken token) {
            _token = token;
            BeginTask(_asyncTask = (act));
        }
        #endregion

        internal readonly object _sync = new object();
        internal void BeginTask(TaskMethod function) {
            _asyncResult = function.BeginInvoke(
                    iAsyncResult => {
                        lock (_sync) {
                            _completed = true;
                            function.EndInvoke(iAsyncResult);
                            Monitor.Pulse(_sync); 
                        }
                    }, null);
                
            _waiter = (timeout, successful) => {
                lock (_sync) {
                    if (!_completed)
                        successful.Val = Monitor.Wait(_sync, timeout);
                }
            };
        }

        #region Wait
        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during
        /// the execution of the <see cref="Task"/>. 
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException"> 
        /// The <see cref="Task"/> has been disposed. 
        /// </exception>
        public void Wait() {
            Wait(Timeout.Infinite, CancellationToken.None);
        }

        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <param name="timeout"> 
        /// A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see 
        /// cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
        /// </param> 
        /// <returns>
        /// true if the <see cref="Task"/> completed execution within the allotted time; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.AggregateException"> 
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>. 
        /// </exception> 
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout"/> is a negative number other than -1 milliseconds, which represents an 
        /// infinite time-out -or- timeout is greater than
        /// <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException"> 
        /// The <see cref="Task"/> has been disposed.
        /// </exception> 
        public bool Wait(TimeSpan timeout) {
            long totalMilliseconds = (long)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > 0x7fffffff)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }

            return Wait((int)totalMilliseconds, CancellationToken.None);
        }


        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution.
        /// </summary>
        /// <param name="cancellationToken"> 
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param> 
        /// <exception cref="T:nucs.Mono.System.Threading.OperationCanceledException"> 
        /// The <paramref name="cancellationToken"/> was canceled.
        /// </exception> 
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>.
        /// </exception> 
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="Task"/> 
        /// has been disposed. 
        /// </exception>
        public void Wait(CancellationToken cancellationToken)
        {
            Wait(Timeout.Infinite, cancellationToken);
        }


        /// <summary> 
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <param name="millisecondsTimeout"> 
        /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to
        /// wait indefinitely.</param>
        /// <returns>true if the <see cref="Task"/> completed execution within the allotted time; otherwise,
        /// false. 
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> 
        /// <paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an 
        /// infinite time-out.
        /// </exception> 
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>.
        /// </exception> 
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="Task"/> 
        /// has been disposed. 
        /// </exception>
        public bool Wait(int millisecondsTimeout) {
            return Wait(millisecondsTimeout, CancellationToken.None);
        }


        /// <summary> 
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <param name="millisecondsTimeout"> 
        /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to
        /// wait indefinitely.
        /// </param>
        /// <param name="cancellationToken"> 
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param> 
        /// <returns> 
        /// true if the <see cref="Task"/> completed execution within the allotted time; otherwise, false.
        /// </returns> 
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>.
        /// </exception> 
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="Task"/> 
        /// has been disposed. 
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> 
        /// <paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an
        /// infinite time-out.
        /// </exception>
        /// <exception cref="T:nucs.Mono.System.Threading.OperationCanceledException"> 
        /// The <paramref name="cancellationToken"/> was canceled.
        /// </exception> 
        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) {
            HandleDisposion();

            if (millisecondsTimeout < -1) {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }

            // Return immediately if we know that we've completed "clean" -- no exceptions, no cancellations 
            if (IsCompleted) return true;

            if (!InternalWait(millisecondsTimeout, cancellationToken))
                return false;

            // If an exception occurred, or the task was cancelled, throw an exception. 
            if (IsCancelled)
                throw new TaskCanceledException();

            //Contract.Assert((m_stateFlags & TASK_STATE_FAULTED) == 0, "Task.Wait() completing when in Faulted state.");

            return true;
        }

        /// <summary>
        /// The core wait function, which is only accesible internally. It's meant to be used in places in TPL code where
        /// the current context is known or cached. 
        /// </summary>
        [MethodImpl(MethodImplOptions.NoOptimization)]  // this is needed for the parallel debugger 
        internal bool InternalWait(int millisecondsTimeout, CancellationToken cancellationToken) {
            var bh = new BoolHolder();
            _waiter(millisecondsTimeout, bh);

            return bh.Val && IsCompleted;
        } 


        #endregion

        public void Dispose() {
            hasDisposed = true;
            _waiter = null;
            _asyncTask = null;
            _asyncResult = null;
        }

        private bool hasDisposed = false;
        private void HandleDisposion() {
            if (hasDisposed)
                throw new ObjectDisposedException("This task is already disposed");
        }

           
    }

    public class Task<R> : IDisposable {

        #region Properties
        public bool IsCompleted {
            get { HandleDisposion(); return _asyncTask != null && _asyncResult.IsCompleted; }
        }
        public R Result {
            get { HandleDisposion(); return _waiter(-1, new BoolHolder()); }
        }

        public CancellationToken CancellationToken { get { return _token; } }
        public bool IsCancelled {
            get { return _token != CancellationToken.None && _token.IsCancellationRequested; }
        }

        internal IAsyncResult _asyncResult;
        internal TaskMethod<R> _asyncTask;

        internal _waitress<R> _waiter;
        internal CancellationToken _token = CancellationToken.None;
        internal bool _completed = false;
        internal Task(TaskMethod<R> act) {
            BeginTask(_asyncTask = (act));
        }

        internal Task(TaskMethod<R> act, CancellationToken token) {
            _token = token;
            BeginTask(_asyncTask = (act));
        }
        #endregion

        internal readonly object _sync = new object();
        internal void BeginTask(TaskMethod<R> function) {
            R retv = default(R);

            _asyncResult = function.BeginInvoke(
                    iAsyncResult => {
                        lock (_sync) {
                            _completed = true;
                            retv = function.EndInvoke(iAsyncResult);
                            Monitor.Pulse(_sync); 
                        }
                    }, null);
                
            _waiter = (timeout, successful) => {
                lock (_sync) {
                    if (!_completed)
                        successful.Val = Monitor.Wait(_sync, timeout);
                    return retv;
                }
            };
        }

        #region Wait
        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during
        /// the execution of the <see cref="Task"/>. 
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException"> 
        /// The <see cref="Task"/> has been disposed. 
        /// </exception>
        public void Wait() {
            Wait(Timeout.Infinite, CancellationToken.None);
        }

        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <param name="timeout"> 
        /// A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see 
        /// cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
        /// </param> 
        /// <returns>
        /// true if the <see cref="Task"/> completed execution within the allotted time; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.AggregateException"> 
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>. 
        /// </exception> 
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout"/> is a negative number other than -1 milliseconds, which represents an 
        /// infinite time-out -or- timeout is greater than
        /// <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException"> 
        /// The <see cref="Task"/> has been disposed.
        /// </exception> 
        public bool Wait(TimeSpan timeout) {
            long totalMilliseconds = (long)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > 0x7fffffff)
            {
                throw new ArgumentOutOfRangeException("timeout");
            }

            return Wait((int)totalMilliseconds, CancellationToken.None);
        }


        /// <summary>
        /// Waits for the <see cref="Task"/> to complete execution.
        /// </summary>
        /// <param name="cancellationToken"> 
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param> 
        /// <exception cref="T:nucs.Mono.System.Threading.OperationCanceledException"> 
        /// The <paramref name="cancellationToken"/> was canceled.
        /// </exception> 
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>.
        /// </exception> 
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="Task"/> 
        /// has been disposed. 
        /// </exception>
        public void Wait(CancellationToken cancellationToken)
        {
            Wait(Timeout.Infinite, cancellationToken);
        }


        /// <summary> 
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <param name="millisecondsTimeout"> 
        /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to
        /// wait indefinitely.</param>
        /// <returns>true if the <see cref="Task"/> completed execution within the allotted time; otherwise,
        /// false. 
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> 
        /// <paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an 
        /// infinite time-out.
        /// </exception> 
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>.
        /// </exception> 
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="Task"/> 
        /// has been disposed. 
        /// </exception>
        public bool Wait(int millisecondsTimeout) {
            return Wait(millisecondsTimeout, CancellationToken.None);
        }


        /// <summary> 
        /// Waits for the <see cref="Task"/> to complete execution. 
        /// </summary>
        /// <param name="millisecondsTimeout"> 
        /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to
        /// wait indefinitely.
        /// </param>
        /// <param name="cancellationToken"> 
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param> 
        /// <returns> 
        /// true if the <see cref="Task"/> completed execution within the allotted time; otherwise, false.
        /// </returns> 
        /// <exception cref="T:System.AggregateException">
        /// The <see cref="Task"/> was canceled -or- an exception was thrown during the execution of the <see
        /// cref="Task"/>.
        /// </exception> 
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="Task"/> 
        /// has been disposed. 
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> 
        /// <paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an
        /// infinite time-out.
        /// </exception>
        /// <exception cref="T:nucs.Mono.System.Threading.OperationCanceledException"> 
        /// The <paramref name="cancellationToken"/> was canceled.
        /// </exception> 
        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) {
            HandleDisposion();

            if (millisecondsTimeout < -1) {
                throw new ArgumentOutOfRangeException("millisecondsTimeout");
            }

            // Return immediately if we know that we've completed "clean" -- no exceptions, no cancellations 
            if (IsCompleted) return true;

            if (!InternalWait(millisecondsTimeout, cancellationToken))
                return false;

            // If an exception occurred, or the task was cancelled, throw an exception. 
            if (IsCancelled)
                throw new TaskCanceledException();

            //Contract.Assert((m_stateFlags & TASK_STATE_FAULTED) == 0, "Task.Wait() completing when in Faulted state.");

            return true;
        }

        /// <summary>
        /// The core wait function, which is only accesible internally. It's meant to be used in places in TPL code where
        /// the current context is known or cached. 
        /// </summary>
        [MethodImpl(MethodImplOptions.NoOptimization)]  // this is needed for the parallel debugger 
        internal bool InternalWait(int millisecondsTimeout, CancellationToken cancellationToken) {
            var bh = new BoolHolder();
            _waiter(millisecondsTimeout, bh);

            return bh.Val && IsCompleted;
        } 


        #endregion

        public void Dispose() {
            hasDisposed = true;
            _waiter = null;
            _asyncTask = null;
            _asyncResult = null;
        }

        private bool hasDisposed = false;
        private void HandleDisposion() {
            if (hasDisposed)
                throw new ObjectDisposedException("This task is already disposed");
        }

           
    }

    internal class BoolHolder {
        public bool Val = false;
    }

    /// <summary> 
    /// Represents an exception used to communicate task cancellation.
    /// </summary> 
    [Serializable] 
    public class TaskCanceledException : OperationCanceledException {
        //[NonSerialized]
        //private Task m_canceledTask; // The task which has been canceled.
 
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException"/> class. 
        /// </summary> 
        public TaskCanceledException() : base("Task has been cancelled by token") { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException"/> 
        /// class with a specified error message.
        /// </summary> 
        /// <param name="message">The error message that explains the reason for the exception.</param> 
        public TaskCanceledException(string message) : base(message)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException"/> 
        /// class with a specified error message and a reference to the inner exception that is the cause of
        /// this exception. 
        /// </summary> 
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param> 
        public TaskCanceledException(string message, Exception innerException) : base(message, innerException)
        {
        }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException"/> class 
        /// with a reference to the <see cref="T:System.Threading.Tasks.Task"/> that has been canceled. 
        /// </summary>
        /// <param name="task">A task that has been canceled.</param> 
        /*public TaskCanceledException(Task task) :
            base("TaskCanceledException_ctor_DefaultMessage", task!=null ? task.CancellationToken:new CancellationToken())
        {
            m_canceledTask = task; 
        }*/
 
        /// <summary> 
        /// Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException"/>
        /// class with serialized data. 
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param>
        protected TaskCanceledException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
        } 
 
        /// <summary>
        /// Gets the task associated with this exception. 
        /// </summary>
        /// <remarks>
        /// It is permissible for no Task to be associated with a
        /// <see cref="T:System.Threading.Tasks.TaskCanceledException"/>, in which case 
        /// this property will return null.
        /// </remarks> 
        /*public Task Task 
        {
            get { return m_canceledTask; } 
        }*/

    }

    public class WaitHandleTimeout : Exception {
        public WaitHandleTimeout() : base() {}

        public WaitHandleTimeout(string message) : base(message) { }
    }
}
#endif