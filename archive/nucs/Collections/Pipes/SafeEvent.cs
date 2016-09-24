using System;
using System.Threading;

namespace nucs.Collections.Concurrent.Pipes {
    /// <summary>
    ///     SafeHandlerInfo - used for storing handler info, plus contains a flag
    ///     indicating if subscribed or not and the reader writer lock
    ///     for when the subscription is read or updated.
    /// </summary>
    /// <typeparam name="TArgs">Event args</typeparam>
    internal class SafeHandlerInfo<TArgs> where TArgs : EventArgs {
        public EventHandler<TArgs> Handler;
        //public object LockObj = new object();

        public ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
        public bool Subscribed = true;

        public SafeHandlerInfo(EventHandler<TArgs> handler) {
            Handler = handler;
        }
    }

    ///     <summary>
    ///         SafeEvent class provides safety for unsubscribing from events so even with
    ///         multiple threads after unsubscribing it will not get called.
    ///         Also makes sure that a null exception won't happen due to the removing of
    ///         events.  Only one event is fired at a time, so single threaded through this event.
    ///     </summary>
    ///     <typeparam name="TArgs">The type of the event args</typeparam>
    public class SafeEvent<TArgs> {
        private readonly object _lock = new object();

        private event Action<TArgs> _event;


        public event Action<TArgs> Event {
            add { Subscribe(value); }

            remove { Unsubscribe(value); }
        }


        /// <summary>
        ///     Used to fire this event from within the class using the SafeEvent
        /// </summary>
        /// <param name="args">The event args</param>
        public virtual void FireEvent(TArgs args) {
            lock (_lock) {
                var localEvents = _event;
                localEvents?.Invoke(args);
            }
        }


        /// <summary>
        ///     Unsubscribe - internally used to unsubscribe a handler from the event
        /// </summary>
        /// <param name="unsubscribeHandler">The handler being unsubscribed</param>
        public void Unsubscribe(Action<TArgs> unsubscribeHandler) {
            lock (_lock) {
                _event -= unsubscribeHandler;
            }
        }

        /// <summary>
        ///     Subscribe - Called to subscribe the handler
        /// </summary>
        /// <param name="eventHandler">The handler to subscribe</param>
        public void Subscribe(Action<TArgs> eventHandler) {
            lock (_lock) {
                _event += eventHandler;
            }
        }
    }
}