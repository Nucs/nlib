using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.Cryptography
{
    /// <summary>
    ///     Shuffles data with two keys
    /// </summary>
    public class NucsShuffler : BaseEncodingCrypter {
        public override string Encrypt(string data, string key) {
            return new string(Encrypt(data.ToCharArray(), key));
        }

        public override string Decrypt(string data, string key) {
            return new string(Decrypt(data.ToCharArray(), key));
        }

        public static T[] Encrypt<T>(IEnumerable<T> data, string key) {
            var seed = Math.Abs(key.GetHashCode()) % 100000;
            var rand = new Random(seed);
            var res = data.ToList();
            var l = new List<Swap>(); //swapped already.
            for (int i = 0; i < seed; i++) {
                var a = rand.Next(0, res.Count);
                var b = rand.Next(0, res.Count);
                l.Add(new Swap(a,b));
            }

            foreach (var swap in l) {
                swap.DoSwap(res);
            }

            return res.ToArray();
        }

        public static T[] Decrypt<T>(IEnumerable<T> data, string key) {
            var seed = Math.Abs(key.GetHashCode()) % 100000;
            var rand = new Random(seed);
            var res = data.ToList();
            var l = new List<Swap>(); //swapped already.
            for (int i = 0; i < seed; i++) {
                var a = rand.Next(0, res.Count);
                var b = rand.Next(0, res.Count);
                l.Add(new Swap(a, b));
            }

            l.Reverse();

            foreach (var swap in l) {
                swap.DoSwap(res);
            }

            return res.ToArray();
        }

        private class Swap {
            public int A { get; set; }
            public int B { get; set; }
            public Swap(int a, int b) {
                A = a;
                B = b;
            }

            public void DoSwap(StringBuilder p) {
                var tmp = p[B];
                p[B] = p[A];
                p[A] = tmp;
            }

            public void DoSwap<T>(IList<T> p) {
                var tmp = p[B];
                p[B] = p[A];
                p[A] = tmp;
            }

        }

        
    }

    
}
