/// <summary>
///     Used to execute code remotely
/// </summary>
/// <typeparam name="T">Type, will be casted to it after execution.</typeparam>
public interface IExecute<out T> {
    T Execute();
}

/// <summary>
///     Used to execute code remotely
/// </summary>
public interface IExecute {
    void Execute();
}
