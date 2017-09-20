using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nito.AsyncEx
{
    /// <summary>
    /// An async-compatible producer/consumer queue.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the queue.</typeparam>
    public interface IAsyncProducerConsumerQueue<T> : IEnumerable<T>
    {
        #region Enqueue
        /// <summary>
        /// Enqueues an item to the producer/consumer queue. This method may block the calling thread. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        void Enqueue(T item);

        /// <summary>
        /// Enqueues an item to the producer/consumer queue. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding. This method may block the calling thread.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the enqueue operation.</param>
        void Enqueue(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Enqueues an item to the producer/consumer queue. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        Task EnqueueAsync(T item);

        /// <summary>
        /// Enqueues an item to the producer/consumer queue. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the enqueue operation.</param>
        Task EnqueueAsync(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to enqueue an item to the producer/consumer queue. Returns <c>false</c> if the producer/consumer queue has completed adding. This method may block the calling thread.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        bool TryEnqueue(T item);

        /// <summary>
        /// Attempts to enqueue an item to the producer/consumer queue. Returns <c>false</c> if the producer/consumer queue has completed adding. This method may block the calling thread.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the enqueue operation.</param>
        bool TryEnqueue(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to enqueue an item to the producer/consumer queue. Returns <c>false</c> if the producer/consumer queue has completed adding.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        Task<bool> TryEnqueueAsync(T item);

        /// <summary>
        /// Attempts to enqueue an item to the producer/consumer queue. Returns <c>false</c> if the producer/consumer queue has completed adding.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the enqueue operation.</param>
        Task<bool> TryEnqueueAsync(T item, CancellationToken cancellationToken);
        #endregion Enqueue

        #region Dequeue
        /// <summary>
        /// Dequeues an item from the producer/consumer queue. Returns the dequeued item. This method may block the calling thread. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding and is empty.
        /// </summary>
        /// <returns>The dequeued item.</returns>
        T Dequeue();

        /// <summary>
        /// Dequeues an item from the producer/consumer queue. Returns the dequeued item. This method may block the calling thread. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding and is empty.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the dequeue operation.</param>
        T Dequeue(CancellationToken cancellationToken);

        /// <summary>
        /// Dequeues an item from the producer/consumer queue. Returns the dequeued item. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding and is empty.
        /// </summary>
        /// <returns>The dequeued item.</returns>
        Task<T> DequeueAsync();

        /// <summary>
        /// Dequeues an item from the producer/consumer queue. Returns the dequeued item. Throws <see cref="InvalidOperationException"/> if the producer/consumer queue has completed adding and is empty.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the dequeue operation.</param>
        /// <returns>The dequeued item.</returns>
        Task<T> DequeueAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to dequeue an item from the producer/consumer queue. This method may block the calling thread.
        /// </summary>
        AsyncProducerConsumerQueue<T>.DequeueResult TryDequeue();

        /// <summary>
        /// Attempts to dequeue an item from the producer/consumer queue. This method may block the calling thread.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the dequeue operation.</param>
        AsyncProducerConsumerQueue<T>.DequeueResult TryDequeue(CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to dequeue an item from the producer/consumer queue.
        /// </summary>
        Task<AsyncProducerConsumerQueue<T>.DequeueResult> TryDequeueAsync();

        /// <summary>
        /// Attempts to dequeue an item from the producer/consumer queue.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the dequeue operation.</param>
        Task<AsyncProducerConsumerQueue<T>.DequeueResult> TryDequeueAsync(CancellationToken cancellationToken);
        #endregion Dequeue

        #region CompleteAdding
        /// <summary>
        /// Synchronously marks the producer/consumer queue as complete for adding.
        /// </summary>
        void CompleteAdding();

        /// <summary>
        /// Asynchronously marks the producer/consumer queue as complete for adding.
        /// </summary>
        [Obsolete("Use CompleteAdding() instead.")]
        Task CompleteAddingAsync();

        /// <summary>
        /// Gets whether this <see cref="AsyncProducerConsumerQueue{T}"/> has been marked as
        /// complete for adding.
        /// </summary>
        bool IsAddingCompleted { get; }

        /// <summary>
        /// Gets whether this <see cref="AsyncProducerConsumerQueue{T}"/> has been marked as
        /// complete for adding and is empty.
        /// </summary>
        bool IsCompleted { get; }
        #endregion CompleteAdding

        #region GetConsumingEnumerable
        /// <summary>
        /// Provides a (synchronous) consuming enumerable for items in the producer/consumer queue.
        /// </summary>
        IEnumerable<T> GetConsumingEnumerable();

        /// <summary>
        /// Provides a (synchronous) consuming enumerable for items in the producer/consumer queue.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the synchronous enumeration.</param>
        IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken);
        #endregion GetConsumingEnumerable

        #region OutputAvailable
        /// <summary>
        /// Asynchronously waits until an item is available to dequeue. Returns <c>false</c> if the producer/consumer queue has completed adding and there are no more items.
        /// </summary>
        Task<bool> OutputAvailableAsync();

        /// <summary>
        /// Asynchronously waits until an item is available to dequeue. Returns <c>false</c> if the producer/consumer queue has completed adding and there are no more items.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to abort the asynchronous wait.</param>
        Task<bool> OutputAvailableAsync(CancellationToken cancellationToken);
        #endregion OutputAvailable
    }
}
