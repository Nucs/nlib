using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using nucs.Collections;

namespace nucs.SystemCore {
    [DebuggerStepThrough]
    public class bit {
        internal bool Bool_Value = false;
        public int Value { get { return (Bool_Value ? 1 : 0); } }

        public bit(bool value) {
            this.Bool_Value = value;
        }

        public static implicit operator bit(bool b) {
            return new bit(b);
        }

        public static implicit operator bit(int i) {
            if (i==1) return new bit(true);
            if (i==0) return new bit(false);
            throw new InvalidOperationException("Failed converting integer to bit - could not convert "+i+" to bit");
        }

        public static implicit operator int(bit i) {
            return (i.Value);
        }

        public static implicit operator bool(bit i) {
            return (i.Bool_Value);
        }

        public static bit operator +(bit c1, bit c2) {
            return new bit(c1.Bool_Value = (c1.Value + c2.Value) >= 1);
        }

        public static bit operator -(bit c1, bit c2) {
            return new bit((c1.Value - c2.Value) >= 1);
        }

        public static bit operator &(bit c1, bit c2) {
            return new bit((c1.Value & c2.Value) >= 1);
            
        }

        public static bit operator |(bit c1, bit c2) {
            return new bit((c1.Value | c2.Value) >= 1);

        }

        public static bit operator *(bit c1, bit c2) {
            return new bit((c1.Value & c2.Value) >= 1);
        }

        public static bit operator ^(bit c1, bit c2) {
            return new bit(c1.Bool_Value && !c2.Bool_Value || !c1.Bool_Value && c2.Bool_Value);
        }


        protected bool Equals(bit other) {
            return Value == other.Value;
        }

        public override int GetHashCode() {
            return Value;
        }

        public override bool Equals(object obj) {
            var s = obj as bit;
            return s != null && s.Bool_Value == Bool_Value;
        }

        public override string ToString() {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public static class BitConversion {
        public static byte ToByte(this bit[] input) {
            var ba = new BitArray(input.ToBooleans());
            var ret = new byte[Convert.ToInt32(Math.Ceiling(ba.Length / 8d))];
            ba.CopyTo(ret, 0);
            return ret[0];
        }

        public static bool[] ToBooleans(this bit[] input) {
            return input.Select(i => i.Bool_Value).ToArray();
        }

        public static IEnumerable<bit> ToBits(this string b) {
            b = new string(b.Reverse().ToArray());
            var r = new ImprovedList<bit>();
            r = b.Aggregate(r, (current, c) => current + (c == '1' ? new bit(true) : c == '0' ? new bit(false) : null));
            r.RemoveAll(bit => bit == null);
            return r;
        }
    }
}
