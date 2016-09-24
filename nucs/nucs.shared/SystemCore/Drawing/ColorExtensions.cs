#if !NET4
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace nucs.SystemCore.Drawing {
    public static class ColorExtensions {

        /// <summary>
        /// Calculates the 'distance' of rgb, giving a sum number that ranges between 0 to 441.6729559 .
        /// </summary>
        public static double Pythagorean3D(this Color c) {
            return Math.Sqrt(c.R*c.R + c.G*c.G + c.B*c.B);
        }
    }
}

#endif