using System;
using nucs.Collections.Extensions;
#if NET_4_5
using System.Threading;
using System.Threading.Tasks;
#else
using System.Threading;
using nucs.Mono.System.Threading;
using Task = nucs.Mono.System.Threading.Task;
#endif

namespace nucs.Utils {

    /// <summary>
    /// Zero based counter, that eventually invokes a method depending on user's configuration.
    /// </summary>
    public class CountingInvoker {
        private bool _autoReset;
        public uint Count { get; set; } 
        public uint Limit { get; set; } 
        public Action ToCall { get; set; }
        public bool AutoReset {
            get { return _autoReset; }
            set { _autoReset = value; if (value) invoked = false; }
        }

        private bool invoked { get; set; }

        /// <param name="limit">The number of counts (not zero based)</param>
        /// <param name="start">Zero based counting start.</param>
        /// <param name="autoReset">Decides wether when method invokes, should Count reset to 0 or wait for manual Reset(); </param>
        /// <param name="toCall">Action to be called on end.</param>
        public CountingInvoker(uint limit, uint start, bool autoReset, Action toCall) {
            Limit = limit;
            Count = start;
            ToCall = toCall;
            AutoReset = autoReset;
        }

        /// <param name="limit">The number of counts (not zero based)</param>
        /// <param name="start">Zero based counting start.</param>
        /// <param name="autoReset">Decides wether when method invokes, should Count reset to 0 or wait for manual Reset(); </param>
        /// <param name="toCall">Actions To be called on end.</param>
        public CountingInvoker(uint limit, uint start, bool autoReset, params Action[] toCall) {
            Limit = limit;
            Count = start;
            toCall.ForEach(k => ToCall += k);
            AutoReset = autoReset;
        }


        /// <param name="limit">The number of counts (not zero based)</param>
        /// <param name="autoReset">Decides wether when method invokes, should Count reset to 0 or wait for manual Reset(); </param>
        /// <param name="toCall">Action to be called on end.</param>
        public CountingInvoker(uint limit, bool autoReset, Action toCall) {
            Limit = limit;
            Count = 0;
            ToCall = toCall;
            AutoReset = autoReset;
        }

        //
        // 0...10
        // 0->1->2->3->4->5->6->7->8->9->10
        // BOOM 10! INVOKE!!


        /// <summary>
        /// Moves up untill it hits the chosen Limit and then invokes the wanted method. returns true on invoke.
        /// </summary>
        /// <returns>has been invoked</returns>
        public bool Up() {
            if (invoked == false)
                Count++;
            if (Count >= Limit - 1) {
                if (invoked == false && ToCall != null)
                    ToCall.Invoke();
                if (AutoReset)
                    Count = 0;
                else
                    invoked = true;
                //Count = Limit - 1;
                return true;
            }
            return false;
        }

        public bool UpAsync() {
            if (invoked == false)
                Count++;
            if (Count >= Limit - 1) {
                if (invoked == false && ToCall != null)
                    Task.Run(()=>ToCall.Invoke());
                if (AutoReset)
                    Count = 0;
                else
                    invoked = true;
                return true;
            }
            return false;
        }

        public void Down() {
            if (Count <= 0)
                return;
            Count--;
        }

        public void Reset() {
            invoked = false;
            Count = 0;
        }
    }
}