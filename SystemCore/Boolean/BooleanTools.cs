using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.SystemCore.Boolean {
    public static class BooleanTools {
        public static bool Not(this bool sample) {
            return !sample;
        }

        public static Bool Not(this Bool sample) {
            sample.value = !sample.value;
            return sample;
        }
    }
}
