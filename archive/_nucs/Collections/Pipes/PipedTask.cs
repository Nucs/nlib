using System;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Collections.Concurrent.Pipes {
    public class PipedTask<T> : Task<T>, IPipe<T> {
        private PipedLine<T> pipe { get; } = new PipedLine<T>();

        /// <summary>
        ///     When item comes in, call this method! - Required!
        /// </summary>
        /// <param name="o"></param>
        public void IncomingItem(T o) {
            pipe.IncomingItem(o);
        }

        /// <summary>
        ///     Connects the current pipe to the end of the give pipe so any objects that will flow to the given pipe, will be
        ///     added to the this pipe.
        /// </summary>
        public void ConnectTo(IPipe<T> pipe) {
            this.pipe.ConnectTo(pipe);
        }

        /// <summary>
        ///     Connects the current pipe to the start of the give pipe so any objects that will flow to this pipe, will be added to the given pipe.
        /// </summary>
        public void ConnectFrom(IPipe<T> pipe) {
            this.pipe.ConnectFrom(pipe);
        }

        public SafeEvent<T> OutgoingPipes => pipe.OutgoingPipes;

        #region Constructors

        private void createListener() {
            this.ContinueWith(t => pipe.OutgoingPipes.FireEvent(t.Result));
        }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<T> function) : base(function) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to this task.</param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created<paramref name=" cancellationToken" /> has already been disposed.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<T> function, CancellationToken cancellationToken) : base(function, cancellationToken) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and creation options.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<T> function, TaskCreationOptions creationOptions) : base(function, creationOptions) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and creation options.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
        /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created<paramref name=" cancellationToken" /> has already been disposed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<T> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken, creationOptions) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and state.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="state">An object representing data to be used by the action.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<object, T> function, object state) : base(function, state) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="state">An object representing data to be used by the function.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to the new task.</param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created<paramref name=" cancellationToken" /> has already been disposed.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<object, T> function, object state, CancellationToken cancellationToken) : base(function, state, cancellationToken) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="state">An object representing data to be used by the function.</param>
        /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<object, T> function, object state, TaskCreationOptions creationOptions) : base(function, state, creationOptions) { createListener(); }

        /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.</summary>
        /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
        /// <param name="state">An object representing data to be used by the function.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to the new task.</param>
        /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created<paramref name=" cancellationToken" /> has already been disposed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is null.</exception>
        public PipedTask(Func<object, T> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, state, cancellationToken, creationOptions) { createListener(); }

        #endregion
    }
}