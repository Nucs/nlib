using System;
using System.Threading;
using System.Threading.Tasks;

namespace nucs.Threading.FastThreadPool {
    /// <summary>
    ///     Task that waits for you to call it!
    /// </summary>
    internal class HaltedTask : IDisposable, IEquatable<HaltedTask> {
        /// <summary>
        ///     The seconds it will take to a task to be marked as timeout
        /// </summary>
        public static int ExpireSeconds = 30;

        private readonly Syncer<Action> rst = new Syncer<Action>();
        private DateTime expiresOn = new DateTime();

        private readonly CancellationTokenSource src = new CancellationTokenSource();
        private readonly Task task;
        private readonly Guid _guid = Guid.NewGuid();

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public HaltedTask(bool canExpire=false) : this(new TaskFactory(TaskScheduler.Default)) {
            CanExpire = canExpire;
        }

        public HaltedTask(TaskFactory factory,bool canExpire= false) {
            CanExpire = canExpire;
#if NET4 || NET45
            task = factory.StartNew(TaskerCore, TaskCreationOptions.LongRunning);
#else
            task = factory.StartNew(TaskerCore, TaskCreationOptions.RunContinuationsAsynchronously | TaskCreationOptions.LongRunning);
#endif
        }

        /// <summary>
        ///     Tells whether the HaltedTask has reached expiriation time. Nothing happens when expires, dispose to close the task.
        /// </summary>
        public bool HasExpired => DateTime.Now > expiresOn;

        /// <summary>
        ///     Defined if this HaltedTask can expire. If one expires, nothing happens but the fact that HasExpired will return true.
        /// </summary>
        public bool CanExpire { get; set; }

        public bool IsAvailable => !rst.IsSet;

        private
#if !NET4
        async
#endif
            void TaskerCore() {
            while (true) {
#if NET4
                var act = rst.WaitAsync().Result;
#else
                var act = await rst.WaitAsync();
#endif
                if (src.IsCancellationRequested)
                    break;
                try {
                    act?.Invoke();
                } catch {} //suppress any exception that went unhandled

                if (CanExpire)
                    expiresOn = DateTime.Now.AddSeconds(ExpireSeconds);
                    
                rst.Reset();
            }
        }

        /// <summary>
        ///     Takes this task and executes the given action, note that this method is not threadsafe!
        /// </summary>
        /// <param name="act"></param>
        public void Take(Action act) {
            if (!IsAvailable)
                throw new InvalidOperationException("Can't take a HaltedTask that is already taken! Check IsAvailable first!! (This method is not safe thread!)");
            rst.Set(act);
        }

#region Overrides
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            src.Cancel();
            rst.Set(null);
            rst.Dispose();
            task.Dispose();
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(HaltedTask other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return _guid.Equals(other._guid);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((HaltedTask) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            return _guid.GetHashCode();
        }

        public static bool operator ==(HaltedTask left, HaltedTask right) {
            return Equals(left, right);
        }

        public static bool operator !=(HaltedTask left, HaltedTask right) {
            return !Equals(left, right);
        }

#endregion
    }
}