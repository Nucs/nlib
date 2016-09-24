#if !(NET35 || NET3 || NET2)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using nucs.SystemCore;
using nucs.Collections.Extensions;
using nucs.SystemCore.Boolean;

#if NET4_5
using System.Threading;
using System.Threading.Tasks;
#else
using System.Threading;
using nucs.Mono.System.Threading;
#endif

namespace nucs.Collections {

    /// <summary>
    ///     A list that have item-add handling using events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerStepThrough]    
    public sealed class ImprovedSortedSet<T> : SortedSet<T> {
        public delegate bool CompareItems(T item);
        public delegate void ItemAddedHandler(T Item);
        public delegate void ItemToBeAddedHandler(T Item, Bool Approval);
        public delegate void ListAccessedHandler();
        /// <summary>
        ///     Post approval->item added to list
        /// </summary>
        public event ItemAddedHandler ItemAdded = null;
        /// <summary>
        ///     Will ask for approval for adding the item to the list
        /// </summary>
        public event ItemToBeAddedHandler ItemToBeAdded = null;
        /// <summary>
        ///     Will invoke when item has been accessed. Read, written, added, delete and so on.
        /// </summary>
        public event ListAccessedHandler ListAccessed = null; 

        public bool IsEmpty { get { return Count == 0; } }

        public ImprovedSortedSet(IEnumerable<T> source) : base(source) {}
        public ImprovedSortedSet() : base() {}
        public ImprovedSortedSet(IComparer<T> comparer) : base(comparer) {}


        public T this[long index] {
            get {
                invokeAccessed();
                return this.Skip(Convert.ToInt32(index)).Take(1).ToArray()[0];
            }
        }

        public new void Add(T item) { //DONE
            if (ItemToBeAdded == null) {
                //No need to check approval
                base.Add(item);
                if (ItemAdded != null)
                    ItemAdded(item);
                return;
            }
            //Check for approval
            var approval = (Bool)true;
            ItemToBeAdded(item, approval);

            if (approval == false)
                return;

            base.Add(item);
            if (ItemAdded != null)
                ItemAdded(item);
            invokeAccessed();
        }

        private void addrange(IEnumerable<T> range) {
            foreach (var r in range) {
                base.Add(r);
            }
        }

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
        public new void AddRange(IEnumerable<T> range) {
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
            var items = range.ToArray();
            if (ItemToBeAdded == null) {
                //No need to be checked
                addrange(items);
                if (ItemAdded != null)
                    Task.Run(() => items.ForEach(item => ItemAdded(item)));
                return;
            }

            var approved = new List<T>();
            foreach (var item in items) {
                var app = (Bool) true;
                ItemToBeAdded(item, app);
                if (app)
                    approved.Add(item);
            }
            addrange(approved);
            if (ItemAdded != null)
                Task.Run(() => items.ForEach(item => ItemAdded(item)));
        }

        public T TakeFirst() {
            return Take(0);
        }

        public T TakeLast() {
            return Take(Count - 1);
        }

        public T Take(int index) {
            if (IsEmpty)
                throw new InvalidOperationException("Count equals to 0, can't take what does not exist.");
            if (0 > index || index >= Count)
                throw new ArgumentOutOfRangeException("index", index, "index is outside the bounds of source array");
            var item = this[index];
            Remove(item);
            invokeAccessed();
            return item;
        }

        public bool[] RemoveMultiple(IEnumerable<T> range) {
            return range.Select(Remove).ToArray();
        }

        /// <summary>
        ///     Waits for arrival of an item for <param name="timeout"> milliseconds. </param>
        /// </summary>
        /// <param name="comparer">The comparator for indentifying the wanted item</param>
        /// <param name="timeout">The time in milliseconds to wait. set it to -1 for infinite</param>
        /// <returns></returns>
        public T WaitFor(CompareItems comparer, int timeout = -1) { //todo test it
            T res = default(T); //ignore default(T), it doesnt matter anyway
            var firstOrDefault = this.FirstOrDefault(t=>comparer(t));
            if (firstOrDefault != null && firstOrDefault.Equals(default(T)) == false)
                return firstOrDefault;
            //not found.. wait for it.
            var waiter = new ManualResetEventSlim(false);
           
            var itemAddedHandler = new ItemAddedHandler(item => { if (comparer(item)) {res = item; waiter.Set();} });
            ItemAdded += itemAddedHandler;
            var fod = this.FirstOrDefault(t => comparer(t));
            if (fod != null && fod.Equals(default(T)) == false) {
                ItemAdded -= itemAddedHandler;
                return fod;
            }
                
            if (timeout > -1)
                waiter.Wait(timeout);
            else 
                waiter.Wait();
            return res;

        }


        public void ClearEventRegisterations() {
            ItemAdded = null;
            ItemToBeAdded = null;
            ListAccessed = null;
        }

        private void invokeAccessed() {
            if (ListAccessed != null)
                Task.Run(() => ListAccessed());
        }

        #region Operators
        public static ImprovedSortedSet<T> operator +(ImprovedSortedSet<T> source, IEnumerable<T> toAdd) {
            source.AddRange(toAdd);
            return source;
        } 

        public static ImprovedSortedSet<T> operator +(ImprovedSortedSet<T> source, T item) {
            source.Add(item);
            return source;
        }

        public static ImprovedSortedSet<T> operator -(ImprovedSortedSet<T> source, IEnumerable<T> ToRemove) {
            foreach (var item in ToRemove)
                source.Remove(item);
            return source;
        }

        public static ImprovedSortedSet<T> operator -(ImprovedSortedSet<T> source, T item) {
            source.Remove(item);
            return source;
        }

        #endregion

    }
}
#endif