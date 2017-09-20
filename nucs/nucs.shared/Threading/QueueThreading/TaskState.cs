namespace nucs.Threading {
    public enum TaskState {
        Idle,
        Queued,
        Executing,
        Executed,
        Cancelled
    }
}