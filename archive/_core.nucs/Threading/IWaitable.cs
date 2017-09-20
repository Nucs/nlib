using System;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace nucs.Threading {
    /// <summary>
    ///     Represents a class that serves a waiting option.
    /// </summary>
     public interface IWaitable {
         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task WaitAsync();

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<bool> WaitAsync(int millisecondsTimeout);

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken);

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<bool> WaitAsync(TimeSpan timespan);

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<bool> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken);

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task WaitAsync(CancellationToken cancellationToken);

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         void Wait();

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         bool Wait(int milliseconds);

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         bool Wait(TimeSpan timespan);

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         /// <param name="cancellationToken">
         ///     The cancellation token used to cancel the wait. If this token is already canceled, this
         ///     method will first check whether the event is set.
         /// </param>
         void Wait(CancellationToken cancellationToken);
     }
}