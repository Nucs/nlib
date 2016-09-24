#if NET4
using Microsoft.Runtime.CompilerServices;
#else
using System.Runtime.CompilerServices;
#endif
using System;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Collections.Concurrent.Pipes {
    public class PipeTask<T> : Pipe<T>
#if !NET4
        , INotifyCompletion 
#endif
    {
        private readonly Task<T> task;

        /// <summary>
        ///     Connects the current pipe to the start of the give pipe so any objects that will flow to this pipe, will be added to the given pipe.
        /// </summary>
        public override void ConnectFrom(IPipe<T> pipe) {
            base.ConnectFrom(pipe);
        }

#region Constructors

        public PipeTask(Task<T> task) {
            this.task = task;
            task.ContinueWith(t => OutgoingPipes.FireEvent(t.Result));
        }
        public TaskAwaiter<T> GetAwaiter() => this.task.GetAwaiter();
        #endregion
#if !NET4

        /// <summary>Schedules the continuation action that's invoked when the instance completes.</summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is null (Nothing in Visual Basic).</exception>
        public void OnCompleted(Action continuation) {
            continuation();
        }
#endif
    }
}