using System;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Threading {

    /// <summary>
    ///     Interface representing an object that can serve an object upon request with waiting options. much like asking for an hamburger in a restaurant - wait for too long and you'll simply walk away without a hamburger.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWaitress<T> {
        #region Waits

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<T> WaitAsync();

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<T> WaitAsync(int millisecondsTimeout);

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        Task<T> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        Task<T> WaitAsync(TimeSpan timespan);

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<T> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken);

         /// <summary>
         ///     Asynchronously waits for this event to be set.
         /// </summary>
         Task<T> WaitAsync(CancellationToken cancellationToken);

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         T Wait();

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         T Wait(int milliseconds);

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         T Wait(TimeSpan timespan);

         /// <summary>
         ///     Synchronously waits for this event to be set. This method may block the calling thread.
         /// </summary>
         /// <param name="cancellationToken">
         ///     The cancellation token used to cancel the wait. If this token is already canceled, this
         ///     method will first check whether the event is set.
         /// </param>
         T Wait(CancellationToken cancellationToken);

         #endregion
     }
}