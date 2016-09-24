using System;

namespace nucs.Collections.Concurrent.Pipes {
    public class PipeEvent<T> : Pipe<T> {
        public PipeEvent() {}

        public PipeEvent(IPipe<T> receiveFrom) {
            this.ConnectFrom(receiveFrom);
        }

        /// <summary>
        /// This event is called when an object comes in.
        /// </summary>
        public event Action<T> Incoming;

        /// <summary>
        ///     When item comes in, call this method! - Required!
        /// </summary>
        /// <param name="o"></param>
        public override void IncomingItem(T o) {
            base.IncomingItem(o);
            Incoming?.Invoke(o);
        }
    }

}
