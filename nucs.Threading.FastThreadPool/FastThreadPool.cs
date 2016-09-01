using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Concurrent.Pipes;

namespace nucs.Threading.FastThreadPool {
    public class FastThreadPool {
        public int TasksAmount { get; set; }
        private Thread manager;
        private readonly PipedConcurrentQueue<Action> syncer = new PipedConcurrentQueue<Action>(PipedQueueStyle.OnEnqueue);
        private readonly PipeManualReset<Action> notifier;
        private readonly TaskFactory factory = new TaskFactory(TaskScheduler.Default);
        private readonly List<HaltedTask> Tasks;

        public FastThreadPool(int tasks) {
            TasksAmount = tasks;
            manager = new Thread(ManagerCore) {IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = "FastThreadPoolManager"};
            notifier = new PipeManualReset<Action>(syncer);
            Tasks = new List<HaltedTask>(tasks);
            for (int i = 0; i < TasksAmount; i++)
                Tasks.Add(new HaltedTask(factory,false));
            
            manager.Start();
        }

        /// <summary>
        ///     Enqueues a job
        /// </summary>
        /// <param name="act"></param>
        public void Enqueue(Action act) {
            syncer.Enqueue(act);
        }

        /// <summary>
        ///     Enqueue a monitored job (wraps the action with ManualResetEventSlim
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public ManualResetEventSlim EnqueueMonitored(Action act) {
            var local = act;
            var mr = new ManualResetEventSlim(false);
            act = () => {
                local();
                mr.Set();
            };
            Enqueue(act);
            return mr;
        }

        private void ManagerCore() {
            while (true) {
                notifier.Wait(2);
                if (syncer.IsEmpty) continue;
                Action act;

                while (!syncer.IsEmpty) {
                    act = syncer.DequeueOrDefault();
                    if (act == null)
                        continue;
                    HandleExpires();
                    _relook:
                    for (var i = 0; i < TasksAmount; i++) {
                        if (Tasks[i].IsAvailable) {
                            Tasks[i].Take(act);
                            goto _exit;
                        }
                    }

                    HaltedTask t;
                    Tasks.Add(t=new HaltedTask(factory,true));
                    t.Take(act);
                    _exit:;
                }

                _end:
                notifier.Reset();
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
    }
}