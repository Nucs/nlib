using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Concurrent.Pipes;
using nucs.Threading.Syncers;
using Nito.AsyncEx;

namespace nucs.Threading.FastThreadPool {
    public class FastThreadPool {
        public int TasksAmount { get; set; }
        private Thread manager;
        private readonly TaskFactory factory = new TaskFactory(TaskScheduler.Default);
        private readonly List<HaltedTask> Tasks;

        private readonly DataflowNotifier<Action> flowControl = new DataflowNotifier<Action>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasks">The amount of standby tasks to be always available - note that if the requested amount exceeds the given amount, the tasks pool will increase temporary.</param>
        public FastThreadPool(int tasks) {
            TasksAmount = tasks;
            manager = new Thread(ManagerCore) {IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = "FastThreadPoolManager"};
            Tasks = new List<HaltedTask>(tasks);
            for (int i = 0; i < TasksAmount; i++)
                Tasks.Add(new HaltedTask(factory,false));
            
            manager.Start();

            //warmup to create threads
            Enqueue(() => { });
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Enqueue<object>(() => null);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            EnqueueMonitored(() => {}).Wait();
        }

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
                Tasks.Add(t=new HaltedTask(factory,true));
                t.Take(act);
                _exit:;
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


#region Static

        private static FastThreadPool _default;

        /// <summary>
        ///     The Default FastThreadPool that is used by the current application
        /// </summary>
        public static FastThreadPool Default {
            get { return _default ?? (_default = new FastThreadPool(8)); }
            internal set { _default = value; }
        }



#endregion

    }
}