using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Concurrent.Pipes;
using nucs.Threading.Syncers;
using Nito.AsyncEx;

namespace nucs.Threading.FastThreadPool {
    public class FastThreadPool : TaskScheduler {

        #region Static

        private static readonly object _cache_sync = new object();
        private static FastThreadPool _default = null;

        /// <summary>
        ///     The Default FastThreadPool that is used by the current application
        /// </summary>
        public new static FastThreadPool Default {
            get {
                if (_default != null)
                    return _default;
                lock (_cache_sync) {
                    if (_default == null)
                        return _default = new FastThreadPool(4);
                }
                return _default;
            }
        }

        #endregion


        #region Parameters and Constructor

        /// <summary>
        ///     The amount of tasks that is promised to be ran instantly.<br/> if it is exceeded - a temporary thread will be created.
        /// </summary>
        public int TasksAmount { get; set; }

        private Thread manager;
        private readonly TaskFactory factory = new TaskFactory(TaskScheduler.Default);
        private readonly TaskFactory currentfactory;
        private readonly List<HaltedTask> Tasks;

        private readonly DataflowNotifier<Action> flowControl = new DataflowNotifier<Action>(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasks">The amount of standby tasks to be always available - note that if the requested amount exceeds the given amount, the tasks pool will increase temporary.</param>
        public FastThreadPool(int tasks) {
            currentfactory = new TaskFactory(this);
            TasksAmount = tasks;
            manager = new Thread(ManagerCore) {IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = "FastThreadPoolManager"};
            Tasks = new List<HaltedTask>(tasks);
            for (int i = 0; i < TasksAmount; i++)
                Tasks.Add(new HaltedTask(factory, false));

            manager.Start();

            //warmup to create threads
            Enqueue(() => { });
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Enqueue<object>(() => null);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            EnqueueMonitored(() => { });
            Run(() => false);
        }

        #endregion

        #region Queuing

        /// <summary>
        ///     Enqueues a job
        /// </summary>
        /// <param name="act"></param>
        public void Enqueue(Action act) {
            flowControl.Set(act);
        }

#if !NET4
        public async Task<T> Enqueue<T>(Func<T> act) {
            var mr = new AsyncManualResetEvent(false);
            object pointer = null;
            var local = act;
            Enqueue(() => {
                pointer = local();
                mr.Set();
            });
            await mr.WaitAsync();
            return (T) pointer;
        }
#else
        public Task<T> Enqueue<T>(Func<T> act) {
            var mr = new AsyncManualResetEvent(false);
            object pointer = null;
            var local = act;
            Enqueue(() => {
                pointer = local();
                mr.Set();
            });
            return mr.WaitAsync().ContinueWith(task => (T)pointer);
        }
#endif

        /// <summary>
        ///     Enqueue a monitored job (wraps the action with ManualResetEventSlim
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public IWaitable EnqueueMonitored(Action act) {
            var local = act;
            var mr = new AsyncManualResetEvent(false);
            act = () => {
                local();
                mr.Set();
            };
            Enqueue(act);
            return mr;
        }

        /// <summary>
        ///     Runs a task through the current task scheduler
        /// </summary>
        public Task Run(Action act) {
            return currentfactory.StartNew(act);
        }

        /// <summary>
        ///     Runs a task through the current task scheduler
        /// </summary>
        public Task<T> Run<T>(Func<T> func) {
            return currentfactory.StartNew(func);
        }

        #endregion

        #region Manager Thread Core

        private
#if !NET4
        async
#endif
        void ManagerCore() {
            while (true) {
#if NET4
                var act = (flowControl.WaitAsync()).Result.Result;
#else
                var act = (await flowControl.WaitAsync()).Result;
#endif
                if (act == null)
                    continue;
                HandleExpires(); //kill timeouts
                //locate available:
                for (var i = 0; i < TasksAmount; i++) {
                    if (Tasks[i].IsAvailable) {
                        Tasks[i].Take(act);
                        goto _exit;
                    }
                }
                //no available, allocate one:
                HaltedTask t;
                Tasks.Add(t = new HaltedTask(factory, true));
                t.Take(act);
                _exit:
                ;
            }
        }

        private void HandleExpires() {
            for (int i = 0; i < TasksAmount; i++) {
                if (!Tasks[i].IsAvailable || !Tasks[i].CanExpire || !Tasks[i].HasExpired)
                    continue;
                Tasks[i].Dispose();
                Tasks.Remove(Tasks[i]);
            }
        }

        #endregion

        #region Task Queue

        /// <summary>Queues a <see cref="T:System.Threading.Tasks.Task" /> to the scheduler. </summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be queued.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
        protected override void QueueTask(Task task) {
            this.Enqueue(() => { TryExecuteTask(task); });
        }

        /// <summary>Determines whether the provided <see cref="T:System.Threading.Tasks.Task" /> can be executed synchronously in this call, and if it can, executes it.</summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">A Boolean denoting whether or not task has previously been queued. If this parameter is True, then the task may have been previously queued (scheduled); if False, then the task is known not to have been queued, and this call is being made in order to execute the task inline without queuing it.</param>
        /// <returns>A Boolean value indicating whether the task was executed inline.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <paramref name="task" /> was already executed.</exception>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) {
            return false;
        }

        /// <summary>For debugger support only, generates an enumerable of <see cref="T:System.Threading.Tasks.Task" /> instances currently queued to the scheduler waiting to be executed.</summary>
        /// <returns>An enumerable that allows a debugger to traverse the tasks currently queued to this scheduler.</returns>
        /// <exception cref="T:System.NotSupportedException">This scheduler is unable to generate a list of queued tasks at this time.</exception>
        protected override IEnumerable<Task> GetScheduledTasks() {
            throw new NotSupportedException();
        }

        #endregion
    }
}