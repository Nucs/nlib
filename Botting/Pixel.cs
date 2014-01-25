using System;
using System.Drawing;
using nucs.SystemCore.Drawing;

namespace MuAutomater {
    public struct Pixel : IDisposable {
        public Byte? Blue;
        public Byte? Green;
        public Byte? Red;

        public Pixel(byte red, byte green, byte blue) {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public void Dispose() {
            Red = null;
            Green = null;
            Blue = null;
        }

        /// <summary>
        /// Calculates the 'distance' of rgb, giving a sum number that ranges between 0 to 441.6729559 .
        /// </summary>
        public double Pythagorean3D {
            get {
                if (Blue == null || Green == null || Red == null) return 0;
                return Math.Sqrt((Red*Red + Green*Green + Blue*Blue).Value);
            }
        }

        public Color ToColor() { return Color.FromArgb(Red ?? 0, Green ?? 0, Blue ?? 0); }

        public static Pixel FromColor(Color color) { return new Pixel(color.R, color.G, color.B); }

        

        public override int GetHashCode() {
            unchecked {
                int hashCode = Blue.GetHashCode();
                hashCode = (hashCode*397) ^ Green.GetHashCode();
                hashCode = (hashCode*397) ^ Red.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(Pixel other) {
            return Blue == other.Blue && Green == other.Green && Red == other.Red;
        }

        public const double Pythagorean3D_MAX = 441.6729559d;
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Pixel && Equals((Pixel) obj);
        }

        public bool Equals(Color c) {
            return Red == c.R && Green == c.G && Blue == c.B;
        }

        /// <summary>
        ///     Compares this pixel with other pixel using Pythagorean3D of the RGB minus other pixel's Pythagorean3D, and tolerance will tell if how much deviation can be tolerated.
        /// </summary>
        /// <param name="other">Compare to</param>
        /// <param name="tolerance"> how much deviation can be tolerated. Ranges between 0 and 441.6729559 (<see cref="Pythagorean3D_MAX"/>) (</param>
        public bool Compare(Pixel other, double tolerance) {
            if (tolerance < 0) throw new ArgumentException("tolerance cannot be below 0", "tolerance");
            if (tolerance > Pythagorean3D_MAX) throw new ArgumentException("tolerance cannot be above 441.6729559", "tolerance");
            return Math.Abs(Pythagorean3D-other.Pythagorean3D) <= tolerance;
        }

        /// <summary>
        ///     Compares this pixel with other pixel using Pythagorean3D of the RGB minus other pixel's Pythagorean3D, and tolerance will tell if how much deviation can be tolerated in precentage from 441.672955 (<see cref="Pythagorean3D_MAX"/>).
        /// </summary>
        /// <param name="other">Compare to</param>
        /// <param name="precantageDiff"> how much deviation can be tolerated. Ranges between 0 and 100 precents from 441.672955 (<see cref="Pythagorean3D_MAX"/>) (</param>
        public bool Compare(Pixel other, byte precantageDiff) {
            if (precantageDiff > 100) throw new ArgumentException("precantageDiff cannot be above 100%", "precantageDiff");
            if (precantageDiff == 100) return true;
            return Math.Abs(Pythagorean3D - other.Pythagorean3D) <= Pythagorean3D_MAX*(precantageDiff/100d);
        }

        /// <summary>
        ///     Compares this pixel with other pixel using Pythagorean3D of the RGB minus other pixel's Pythagorean3D, and tolerance will tell if how much deviation can be tolerated.
        /// </summary>
        /// <param name="other">Compare to</param>
        /// <param name="tolerance"> how much deviation can be tolerated. Ranges between 0 and 441.6729559 (<see cref="Pythagorean3D_MAX"/>) (</param>
        public bool Compare(Color other, double tolerance) {
            if (tolerance < 0) throw new ArgumentException("tolerance cannot be below 0", "tolerance");
            if (tolerance > Pythagorean3D_MAX) throw new ArgumentException("tolerance cannot be above 441.6729559", "tolerance");
            return Math.Abs(Pythagorean3D - other.Pythagorean3D()) <= tolerance;
        }

        /// <summary>
        ///     Compares this pixel with other pixel using Pythagorean3D of the RGB minus other pixel's Pythagorean3D, and tolerance will tell if how much deviation can be tolerated in precentage from 441.672955 (<see cref="Pythagorean3D_MAX"/>).
        /// </summary>
        /// <param name="other">Compare to</param>
        /// <param name="precantageDiff"> how much deviation can be tolerated. Ranges between 0 and 100 precents from 441.672955 (<see cref="Pythagorean3D_MAX"/>) (</param>
        public bool Compare(Color other, byte precantageDiff) {
            if (precantageDiff > 100) throw new ArgumentException("precantageDiff cannot be above 100%", "precantageDiff");
            if (precantageDiff == 100) return true;
            return Math.Abs(Pythagorean3D - other.Pythagorean3D()) <= Pythagorean3D_MAX*(precantageDiff/100d);
        }

        public override string ToString() {
            return string.Format("({0}|{1}|{2})", Red ?? -1, Green ?? -1, Blue ?? -1);
        }
    }

        
    /*public unsafe struct Pixel : IDisposable {
        public Byte* Red;
        public Byte* Green;
        public Byte* Blue;

        public Pixel(byte red, byte green, byte blue) {
            Red = &red;
            Green = &green;
            Blue = &blue;
        }

        public void Dispose() {
            Red = null;
            Green = null;
            Blue = null;
        }

        public Color ToColor() {
            return Color.FromArgb(*Red, *Green, *Blue);
        }

        public static Pixel FromColor(Color color) {
            return new Pixel(color.R, color.G, color.B);
        }
    }*/

}