using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace nucs.Automation.Imaging.Helpers {
    public static class MathHelper {
        /// <summary>
        ///     Returns number of columns and rows for a given number of cells.
        /// </summary>
        /// <param name="grids">How many X*Y will result in</param>
        public static SamePoint CalculateTableForGrids(int grids) {
            var l = new HashSet<SamePoint>(SamePoint.XYComparer) {new SamePoint(grids, 1)};

            for (int x = 2; x < grids; x++) {
                double y = grids / (x * 1d);
                if (y - Math.Floor(y) > 0)
                    continue;
                l.Add(new SamePoint(x, Convert.ToInt32(y)));
            }

            var @out = l.OrderBy(p => Math.Abs(p.X - p.Y)).First().ToPoint();
            return new SamePoint(Math.Max(@out.X, @out.Y), Math.Min(@out.X, @out.Y));
        }

        [DebuggerDisplay("({X}, {Y})")]
        public class SamePoint : IEquatable<SamePoint> {
            public int X { get; set; }
            public int Y { get; set; }

            public SamePoint(int x, int y) {
                X = x;
                Y = y;
            }

            public Point ToPoint() {
                return new Point(X, Y);
            }

            /// <summary>Serves as the default hash function. </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode() {
                unchecked {
                        var a = Convert.ToInt32(Math.Pow(X, Y) % Int32.MaxValue);
                        var b = Convert.ToInt32(Math.Pow(Y, X) % Int32.MaxValue);
                        return X + Y + a + b;
                    }
            }

            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            public bool Equals(SamePoint other) {
                if (ReferenceEquals(null, other))
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return (X == other.X && Y == other.Y) || (X == other.Y && Y == other.X);
            }

            /// <summary>Determines whether the specified object is equal to the current object.</summary>
            /// <param name="obj">The object to compare with the current object. </param>
            /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != this.GetType())
                    return false;
                return Equals((SamePoint) obj);
            }

            private sealed class XYEqualityComparer : IEqualityComparer<SamePoint> {
                public bool Equals(SamePoint x, SamePoint y) {
                    if (ReferenceEquals(x, y))
                        return true;
                    if (ReferenceEquals(x, null))
                        return false;
                    if (ReferenceEquals(y, null))
                        return false;
                    if (x.GetType() != y.GetType())
                        return false;
                    return (x.X == y.X && x.Y == y.Y) || (x.X == y.Y && x.Y == y.X);
                }

                public int GetHashCode(SamePoint obj) {
                    return obj.GetHashCode();
                }
            }

            private static readonly IEqualityComparer<SamePoint> XYComparerInstance = new XYEqualityComparer();

            public static IEqualityComparer<SamePoint> XYComparer {
                get { return XYComparerInstance; }
            }

            public static bool operator ==(SamePoint left, SamePoint right) {
                return Equals(left, right);
            }

            public static bool operator !=(SamePoint left, SamePoint right) {
                return !Equals(left, right);
            }
        }
    }
}