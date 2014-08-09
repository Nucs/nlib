using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nucs.SystemCore;
using nucs.SystemCore.Boolean;

namespace nucs.Collections {
    public class Collector<T> {
        public delegate void ItemAddedToApproveHandler(T item, int itemnumber, bool isLast, Bool Approve);
        public delegate void ItemAddedApprovedHandler(T item, int itemnumber, bool isLast);

        public event ItemAddedToApproveHandler ItemAddedToApprove;
        public event ItemAddedApprovedHandler ItemAddedApproved;
        public readonly ArrayQueue<T> Queue = new ArrayQueue<T>(); //explanations: http://www.tutorialspoint.com/csharp/csharp_queue.htm
        internal readonly object lock_take = new object();
        /// <summary>
        /// Represents how many items were added regardless to their approval
        /// </summary>
        public int Counter { get; private set; }
        public int ItemsLeft { get { return Queue.Count; } }
        public bool IsClosed { get; private set; }

        public Collector() {
            IsClosed = false;
            Counter = 0;
        }

        private readonly object lock_add = new object();
        public void Add(T item, bool isLast = false) {
            lock (lock_add) {
                if (IsClosed) throw new InvalidOperationException("Cannot add items after closing collector (declaring item added is last)");
                Counter++;
                if (isLast)
                    IsClosed = true;
                if (ItemAddedToApprove == null) goto _approved;
                var approval = (Bool) true;
                ItemAddedToApprove(item, Counter, isLast, approval);
                if (approval == false)
                    return;
                _approved:
                lock (lock_take) Queue.Enqueue(item);
                if (ItemAddedApproved != null) ItemAddedApproved(item, Counter, isLast);
            }
        }

        public T TakeFirst() {
            lock (lock_take) {
                return Queue.Dequeue();
            }
        }

        public T Take(int index) {
            lock (lock_take) {
                return Queue.Take(index);
            }
        }

        public T PeakFirst() {
            lock (lock_take) {
                return Queue.PeekFirst();
            }
        }

        public T Peak(int index) {
            lock (lock_take) {
                return Queue.Peek(index);
            }
        }

        public bool Remove(T item) {
            lock (lock_take) {
                Queue.Remove(item);
            }
            return false;
        }

        public void RemoveAt(int index) {
            lock (lock_take) {
                Queue.RemoveAt(index);
            }
        }

    }

    public class CollectorPump<T> {
        public readonly Collector<T> Collector;
        public delegate void ItemAddedHandler(CollectorPump<T> pump, T item, int itemnumber, bool isLast);
        public object lock_output = new object();
        /// <summary>
        /// Number of items that went through the pump
        /// </summary>
        public int Counter { get; private set; }
        public event ItemAddedHandler ItemReceived;
        public bool IsPumpOpen { get; private set; }
        private bool LastReceived = false;
        public bool IsClosed { get; private set; }
        private readonly Collector<T>.ItemAddedApprovedHandler CollectorAction;
        public CollectorPump(Collector<T> collector, ItemAddedHandler pumpOutput, bool openPump = false) {
            Collector = collector;
            Counter = 0;
            IsClosed = false;
            CollectorAction = (item, itemnumber, last) => Sucker();
            collector.ItemAddedApproved += (item, itemnumber, last) => LastReceived = last; //Last listener
            ItemReceived += pumpOutput;
            if (openPump) OpenPump();
        }

        /// <summary>
        /// Opens the flow to output event
        /// </summary>
        /// <returns></returns>
        public bool OpenPump() {
            lock (lock_output) {
                if (IsPumpOpen) return false;
                Collector.ItemAddedApproved += CollectorAction;
                Sucker();
                return (IsPumpOpen = true);
            }
        }

        public bool ClosePump() {
            lock (lock_output) {
                if (IsPumpOpen == false) return false;
                Collector.ItemAddedApproved -= CollectorAction;
                return !(IsPumpOpen = false);
            }
        }

        private void Sucker() {
            lock (lock_output) {
                T item;
                while (true) {
                    if (Collector.ItemsLeft == 0)
                        break;
                    try {
                        if ((item = Collector.TakeFirst()).Equals(null))
                            break;
                    } catch {
                        break;
                    }
                    Counter++;
                    if (LastReceived && Collector.Queue.Count == 0 && Collector.IsClosed) {
                        ItemReceived(this, item, Counter, true);
                        ClosePump();
                        IsClosed = true;
                        break;
                    }
                    ItemReceived(this, item, Counter, false);
                }
            }
        }
    }
}
