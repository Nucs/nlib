using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nucs.SystemCore;
using nucs.Collections.Extensions;

namespace nucs.Collections {
    /// <summary>
    /// A list that have item-add handling using events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ImprovedList<T> : List<T> {
        public delegate bool CompareItems(T item);
        public delegate void ItemAddedHandler(T Item);
        public delegate void ItemToBeAddedHandler(T Item, Bool Approval);
        public event ItemAddedHandler ItemAdded = null;
        public event ItemToBeAddedHandler ItemToBeAdded = null; 
        public bool IsEmpty { get { return Count == 0; } }

        public ImprovedList(IEnumerable<T> source) : base(source) {}
        public ImprovedList() : base() {}
        public ImprovedList(int capacity) : base(capacity) {}
        public ImprovedList(IList<T> list) {AddRange(list);}

        public T this[long index] {
            get {
                return base[Convert.ToInt32(index)];
            }
            set {
                base[Convert.ToInt32(index)] = value;
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
        }
        public new void AddRange(IEnumerable<T> range) {
            var items = range.ToArray();
            if (ItemToBeAdded == null) {
                //No need to be checked
                base.AddRange(items);
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
            base.AddRange(approved);
            if (ItemAdded != null)
                Task.Run(() => items.ForEach(item => ItemAdded(item)));
        }

        public new void Insert(int index, T item) {
            if (ItemToBeAdded == null) {
                //No need to check approval
                base.Insert(index, item);
                if (ItemAdded != null)
                    ItemAdded(item);
                return;
            }
            //Check for approval
            var approval = (Bool)true;
            ItemToBeAdded(item, approval);

            if (approval == false)
                return;

            base.Insert(index, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }
        public new void InsertRange(int index, IEnumerable<T> range) {
            var items = range.ToArray();
            if (ItemToBeAdded == null) {
                //No need to be checked
                base.InsertRange(index, items);
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
            base.InsertRange(index, approved);
            if (ItemAdded != null)
                Task.Run(() => items.ForEach(item => ItemAdded(item)));
        }

        public T TakeFirst() {
            return Take(0);
        }

        public T TakeLast() {
            var b = new ConcurrentBag<T>();
            return Take(Count - 1);
        }

        public T Take(int index) {
            if (IsEmpty)
                throw new InvalidOperationException("Count equals to 0, can't take what does not exist.");
            if (0 > index || index >= Count)
                throw new ArgumentOutOfRangeException("index", index, "index is outside the bounds of source array");
            var item = this[index];
            RemoveAt(index);
            return item;
        }

        public bool[] RemoveMultiple(IEnumerable<T> range) {
            return range.Select(Remove).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="timeout"></param>
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
        }


        #region Operators
        public static ImprovedList<T> operator +(ImprovedList<T> source, IEnumerable<T> toAdd) {
            source.AddRange(toAdd);
            return source;
        } 

        public static ImprovedList<T> operator +(ImprovedList<T> source, T item) {
            source.Add(item);
            return source;
        }

        public static ImprovedList<T> operator -(ImprovedList<T> source, IEnumerable<T> ToRemove) {
            foreach (var item in ToRemove)
                source.Remove(item);
            return source;
        }

        public static ImprovedList<T> operator -(ImprovedList<T> source, T item) {
            source.Remove(item);
            return source;
        }

        #endregion

    }
}
