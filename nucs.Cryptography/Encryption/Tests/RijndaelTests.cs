using System;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nucs.Cryptography.Tests {
    [TestClass]
    public class RijndaelTests {
        [TestMethod]
        public void RijndaelTest() {
            const int times = 10;
            var rand = new Random();
            var sw = new Stopwatch();
            for (int i = 0; i < times; i++)
            {
                var str = Generate(rand, rand.Next(0, 10000));
                var key = Generate(rand, rand.Next(0, 10000));
                var rij = new RijndaelEnhanced(key);
                sw.Reset();
                sw.Start();
                var enc = rij.EncryptToBytes(str);
                var dec = rij.Decrypt(enc);
                var res = sw.ElapsedMilliseconds;
                Assert.AreEqual(str, dec);
                Debug.WriteLine($"{str.GetHashCode()} == {dec.GetHashCode()} ({res})");
            }
        }

        private static string Generate(Random rand, int len = 10)
        {
            if (len <= 0) return "";
            char ch;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            for (int i = 0; i < len; i++)
            {
                if (rand.Next(1, 3) == 1)
                    builder[i] = char.ToLowerInvariant(builder[i]);
            }
            return builder.ToString();
        }
    }
}