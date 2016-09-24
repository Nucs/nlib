namespace nucs.Collections.Concurrent.Pipes
{
    public interface IPipe<T> {
        /// <summary>
        ///     Connects the current pipe to the end of the given pipe so any objects that will flow to the given pipe, will be added to the this pipe.
        ///     * pipe arg - gives
        ///     * this pipe - takes
        /// </summary>
        void ConnectTo(IPipe<T> pipe);
        /// <summary>
        ///     Connects the current pipe to the start of the given pipe so any objects that will flow to this pipe, will be added to the given pipe.
        ///     * pipe arg - takes
        ///     * this pipe - gives
        /// </summary>
        void ConnectFrom(IPipe<T> pipe);

        SafeEvent<T> OutgoingPipes { get; }

        /// <summary>
        ///     When item comes in, call this method! - Required!
        /// </summary>
        /// <param name="obj"></param>
        void IncomingItem(T obj);
    }
}
