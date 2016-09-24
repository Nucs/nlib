namespace nucs.Threading {
    /// <summary>
    ///     Represents an interface with sync object for locking.
    /// </summary>
    public interface ILockingObject {
        object Sync { get; }
    }
}