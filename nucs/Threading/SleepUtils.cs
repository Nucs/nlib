using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using Thread = System.Threading.Thread;

namespace nucs.Threading
{

    public delegate bool QuestionHandler();

    /// <summary>
    /// Various methods to sleep efficiently and etc.
    /// </summary>
    public static class Sleep {
        /// <summary>
        ///     Sleeps for <paramref name="sleepFor"/> milliseconds while
        ///     every <paramref name="sleepFor"/>/<paramref name="frequency"/> milliseconds the <paramref name="till"/>
        ///     will be checked. If it turns out to be true, then the sleep will stop.
        /// </summary>
        /// <param name="sleepFor">Total amount of time to sleep</param>
        /// <param name="frequency">The factor of how many times during the total time of sleep to check the given Anonymous method return</param>
        /// <param name="till">This Anonymous method suppose to return true when it is ok to stop sleeping. otherwise false meaning keep sleeping</param>
        /// <returns>True if the app was terminated from the Question (<paramref name="till"/>), false if not.</returns>
        public static bool Till(int sleepFor, ushort frequency ,QuestionHandler till) {
            if (frequency > sleepFor) throw new InvalidOperationException("Frequency cannot be less than the time required to sleep in milliseconds");
            var tm = Convert.ToInt32(Math.Ceiling(sleepFor * 1f / frequency));
            for (int i = 0; i < frequency; i++) {
                Thread.Sleep(tm);
                if (till()) return true;
            }
            return false;
        }


    }
}
