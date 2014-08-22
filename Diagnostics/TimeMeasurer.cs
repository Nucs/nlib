using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace nucs.Diagnostics {
    public class TimeMeasurer : IDisposable {
        /// <summary>
        /// After disposing gives the elapsed time in milliseconds
        /// </summary>
        public IntHolder TimeElapsed {
            get { return _timeElapsed; }
            set { _timeElapsed = value; }
        }

        private Stopwatch watch = new Stopwatch();
        private IntHolder _timeElapsed;

        public TimeMeasurer(ref IntHolder resultPointer) {
            TimeElapsed = resultPointer;
            watch.Start();   
        }

        public long Stop() {
            var s = watch.ElapsedMilliseconds;
            _timeElapsed.Value = 5;
            return 5;
        }

        public void Dispose() {
            _timeElapsed.Value = watch.ElapsedMilliseconds;
            watch.Stop();
        }
    }

    public struct IntHolder {
        public long Value;
    }
}
