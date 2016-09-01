using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace nucs.Collections.Concurrent.Pipes {
    /// <summary>
    ///     A conqurrent queue 
    ///  </summary>
    /// <typeparam name="T"></typeparam>
    public class PipedConcurrentQueue<T> : Pipe<T>, IEnumerable<T> {
        public PipedQueueStyle Style { get; set; }

        private readonly ConcurrentQueue<T> queue;

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> class.</summary>
        public PipedConcurrentQueue(PipedQueueStyle style) {
            Style = style;
            queue = new ConcurrentQueue<T>();
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> class that contains elements copied from the specified collection</summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</param>
        /// <param name="style"></param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is null.</exception>
        public PipedConcurrentQueue(IEnumerable<T> collection, PipedQueueStyle style) {
            Style = style;
            queue = new ConcurrentQueue<T>(collection);
        }

        /// <summary>Adds an object to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</summary>
        /// <param name="item">The object to add to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />. The value can be a null reference (Nothing in Visual Basic) for reference types.</param>
        public void Enqueue(T item) {
            queue.Enqueue(item);
            if (Style == PipedQueueStyle.OnEnqueue) {
                this.IncomingItem(item);
            }
        }

        /// <summary>Tries to remove and return the object at the beginning of the concurrent queue.</summary>
        /// <returns>true if an element was removed and returned from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> successfully; otherwise, false.</returns>
        /// <param name="result">When this method returns, if the operation was successful, <paramref name="result" /> contains the object removed. If no object was available to be removed, the value is unspecified.</param>
        public bool Dequeue(out T result) {
            T o;
            var ret = queue.TryDequeue(out o);
            if (ret && Style == PipedQueueStyle.OnDequeue) {
                this.IncomingItem(o);
            }
            result = o;
            return ret;
        }
        /// <summary>
        ///     Returns dequeue or the default value - NOTICE: there is no way to distinguish between enqueued null to default(T) that is null, nor with int 0
        /// </summary>
        /// <returns></returns>
        public T DequeueOrDefault() {
            T @out;
            var r = this.Dequeue(out @out);
            return r ? @out : default(T);
        }

        /// <summary>Tries to return an object from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> without removing it.</summary>
        /// <returns>true if an object was returned successfully; otherwise, false.</returns>
        /// <param name="result">When this method returns, <paramref name="result" /> contains an object from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> or an unspecified value if the operation failed.</param>
        public bool Peek(out T result) {
            return queue.TryPeek(out result);
        }
        
        #region Others

        /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
        public void CopyTo(Array array, int index) {
            ((ICollection) queue).CopyTo(array, index);
        }

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
        public object SyncRoot {
            get { return ((ICollection) queue).SyncRoot; }
        }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized {
            get { return ((ICollection) queue).IsSynchronized; }
        }

        /// <summary>Copies the elements stored in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> to a new array.</summary>
        /// <returns>A new array containing a snapshot of elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
        public T[] ToArray() {
            return queue.ToArray();
        }

        /// <summary>Copies the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" /> -or- The number of elements in the source <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(T[] array, int index) {
            queue.CopyTo(array, index);
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</summary>
        /// <returns>An enumerator for the contents of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
        public IEnumerator<T> GetEnumerator() {
            return queue.GetEnumerator();
        }

        /// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is empty.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is empty; otherwise, false.</returns>
        public bool IsEmpty {
            get { return queue.IsEmpty; }
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
        public int Count {
            get { return queue.Count; }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

#endregion
    }

    /// <summary>
    ///     Decides how the pipe will act
    /// </summary>
    public enum PipedQueueStyle {
        /// <summary>
        ///     When item enqueued, it will be sent to the next pipe
        /// </summary>
        OnEnqueue,
        /// <summary>
        ///     When item dequeued, it will be sent to the next pipe
        /// </summary>
        OnDequeue
    }
}
