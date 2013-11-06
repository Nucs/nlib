using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Threading {
    public static class TaskTools {
        /// <summary>
        /// Might not work.. do not use
        /// </summary>
        public async static Task<T> TimeoutAfter<T>(Task<T> task, int milliseconds, T TimeoutToken) {
            if (task == await Task.WhenAny(task, Task.Delay(milliseconds))) 
                return await task;
            return await Task.Run(() => TimeoutToken);
        }

        public static void WaitAllCancellable(this IEnumerable<Task> Tasks, bool IgnoreAllvsJustCancel = false) {
            var _tasks = Tasks.ToList();
            _rewait:
            try {
                Task.WaitAll(_tasks.ToArray());
            } catch (AggregateException e) {
                var notcancel = e.InnerExceptions.Where(ex => (typeof (TaskCanceledException) != ex.GetType())).ToArray();
                if (notcancel.Length > 0) //count != 0
                    throw new AggregateException("One or more none-cancell exceptions occured, see inner exceptions", notcancel);
                _tasks = _tasks.Where(t => t.IsCanceled || t.Exception == null || t.IsFaulted || t.IsCompleted).ToList();
                goto _rewait;
                
            }
        }


    }
}
