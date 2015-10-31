using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nucs.Cryptography.Tests
{
    [TestClass]
    public class NucsShufflerTests {

        [TestMethod]
        public void TestShuffle() {
            const string data = "potato";
            var enc = NucsShuffler.Encrypt(data, "hi, im a seed");
            var dec = NucsShuffler.Decrypt(enc, "hi, im a seed");
            Assert.AreEqual(data, new string(dec));
        }

        [TestMethod]
        public void LongTestShuffle() {
            const int times = 50;
            var rand = new Random();
            var sw = new Stopwatch();
            for (int i = 0; i < times; i++) {
                var str = Generate(rand, rand.Next(0, 10000));
                var key = Generate(rand, rand.Next(0, 10000));
                sw.Reset();
                sw.Start();
                var enc = NucsShuffler.Encrypt(str, key);
                var dec = NucsShuffler.Decrypt(enc, key);
                var res = sw.ElapsedMilliseconds;
                Assert.IsTrue(str.SequenceEqual(dec));
                Debug.WriteLine($"{str.GetHashCode()} == {dec.GetHashCode()} ({res})");
            }
        }

        [TestMethod]
        public void TestGenericShuffle() {
            var data = "potato".ToCharArray();
            var enc = NucsShuffler.Encrypt(data, "hi, im a seed");
            var dec = NucsShuffler.Decrypt(enc, "hi, im a seed");
            for (int i = 0; i < data.Length; i++) {
                Assert.AreEqual(data[i], dec[i]);
            }
        }

        [TestMethod]
        public void LongTestGenericShuffle() {
            const int times = 50;
            var rand = new Random();
            var sw = new Stopwatch();
            for (int i = 0; i < times; i++) {
                var str = Generate(rand, rand.Next(0, 10000)).ToCharArray();
                var key = Generate(rand, rand.Next(0, 10000));
                sw.Reset();
                sw.Start();
                var enc = NucsShuffler.Encrypt(str, key);
                var dec = NucsShuffler.Decrypt(enc, key);
                var res = sw.ElapsedMilliseconds;
                for (int j = 0; j < str.Length; j++) {
                    Assert.AreEqual(str[j], dec[j]);
                }
                Debug.WriteLine($"({res})");
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
