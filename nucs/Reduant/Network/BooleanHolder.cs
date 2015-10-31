using System.Diagnostics;
using nucs.Annotations;

namespace nucs.Network {
    /// <summary>
    /// holds a bool so it can be passed through events and etc..
    /// </summary>
    public class BooleanHolder {
        private bool value;
        public BooleanHolder() {
            value = false;
        }

        [DebuggerStepThrough]
        public BooleanHolder(bool value) {
            this.value = value;
        }

        public BooleanHolder changeValue(bool boolean) {
            value = boolean;
            return this;
        }

        public static implicit operator bool(BooleanHolder holder) {
            return holder.value;
        }
    }
}