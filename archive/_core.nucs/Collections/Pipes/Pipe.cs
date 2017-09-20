using System;

namespace nucs.Collections.Concurrent.Pipes {
    public abstract class Pipe<T> : IPipe<T> {

        /// <summary>
        ///     When item comes in, call this method! - Required!
        /// </summary>
        /// <param name="o"></param>
        public virtual void IncomingItem(T o) {
            OutgoingPipes.FireEvent(o);
        }

        /// <summary>
        ///     The event to send out incoming item.
        /// </summary>
        public SafeEvent<T> OutgoingPipes { get; } = new SafeEvent<T>();

        /// <summary>
        ///     Connects the current pipe to the end of the give pipe so any objects that will flow to the given pipe, will be
        ///     added to the this pipe.
        /// </summary>
        public virtual void ConnectTo(IPipe<T> pipe) {
            var o = pipe as Pipe<T>;
            if (o == null)
                throw new InvalidCastException("Given pipe does not implmenet class Pipe<T>!");
            OutgoingPipes.Subscribe(o.IncomingItem);
        }

        /// <summary>
        ///     Connects the current pipe to the start of the give pipe so any objects that will flow to this pipe, will be added to the given pipe.
        /// </summary>
        public virtual void ConnectFrom(IPipe<T> pipe) {
            var o = pipe as Pipe<T>;
            if (o == null)
                throw new InvalidCastException("Given pipe does not implmenet class Pipe<T>!");
            o.OutgoingPipes.Subscribe(IncomingItem);
        }
    }
}