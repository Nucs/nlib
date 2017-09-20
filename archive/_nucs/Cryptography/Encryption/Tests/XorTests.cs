using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nucs.Cryptography.Tests {
    [TestClass]
    public class XorTests {
        [TestMethod]
        public void XorTest() {
            const int times = 10;
            var rand = new Random();
            var sw = new Stopwatch();
            for (var i = 0; i < times; i++) {
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

        [TestMethod]
        public void TestShuffle() {
            const string data = "potato";
            var enc = NucsShuffler.Encrypt(data, "hi, im a seed");
            var dec = NucsShuffler.Decrypt(enc, "hi, im a seed");
            Assert.IsTrue(data.ToCharArray().SequenceEqual(dec));
        }

        [TestMethod]
        public void LongTestShuffle() {
            const int times = 50;
            var rand = new Random();
            var sw = new Stopwatch();
            for (var i = 0; i < times; i++) {
                var str = Generate(rand, rand.Next(0, 10000));
                var key = Generate(rand, rand.Next(0, 10000));
                sw.Reset();
                sw.Start();
                var enc = NucsShuffler.Encrypt(str, key);
                var dec = NucsShuffler.Decrypt(enc, key);
                var res = sw.ElapsedMilliseconds;
                var decstr=new string(dec);
                Assert.AreEqual(str, decstr);
                Debug.WriteLine($"{str.GetHashCode()} == {decstr.GetHashCode()} ({res})");
            }
        }

        [TestMethod]
        public void TestGenericShuffle() {
            var data = "potato".ToCharArray();
            var enc = NucsShuffler.Encrypt(data, "hi, im a seed");
            var dec = NucsShuffler.Decrypt(enc, "hi, im a seed");
            for (var i = 0; i < data.Length; i++) {
                Assert.AreEqual(data[i], dec[i]);
            }
        }

        [TestMethod]
        public void LongTestGenericShuffle() {
            var xor = new XorStringEncryption();
            const int times = 50;
            var rand = new Random();
            var sw = new Stopwatch();
            for (var i = 0; i < times; i++) {
                var str = new string(Generate(rand, rand.Next(0, 10000)).ToCharArray());
                var key = Generate(rand, rand.Next(0, 10000));
                sw.Reset();
                sw.Start();
                var enc = xor.Encrypt(str, key);
                var dec = xor.Decrypt(enc, key);
                var res = sw.ElapsedMilliseconds;
                for (var j = 0; j < str.Length; j++) {
                    Assert.AreEqual(str[j], dec[j]);
                }
                Debug.WriteLine($"({res})");
            }
        }

        private static string Generate(Random rand, int len = 10) {
            if (len <= 0) return "";
            char ch;
            var builder = new StringBuilder();
            for (var i = 0; i < len; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            for (var i = 0; i < len; i++) {
                if (rand.Next(1, 3) == 1)
                    builder[i] = char.ToLowerInvariant(builder[i]);
            }
            return builder.ToString();
        }
    }
}