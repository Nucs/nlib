using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nucs.Cryptography;

namespace nucs.Serialization.Tests {
    [TestClass]
    public class EncryptedBinaryTests {
        [TestMethod]
        public void DefaultEncryptedSerializeTest() {
            var obj = new ec();
            var rand = new Random();
            var troj = new byte[rand.Next(10000, 128000)]; rand.NextBytes(troj);
            var app = new byte[rand.Next(10000, 128000)]; rand.NextBytes(app);
            obj.Troj = troj; obj.OriginalApp = app;
            var pass = Guid.NewGuid().ToString();

            var ec = obj.SerializeBinary(pass).DeserializeBinary<ec>(pass);

            Assert.IsTrue(ec.OriginalApp.SequenceEqual(obj.OriginalApp));
            Assert.IsTrue(ec.Troj.SequenceEqual(obj.Troj));
        }

        [TestMethod]
        public void NucsShuffleEncryptedSerializeTest() {
            var obj = new ec();
            var rand = new Random();
            var troj = new byte[rand.Next(10000, 128000)]; rand.NextBytes(troj);
            var app = new byte[rand.Next(10000, 128000)]; rand.NextBytes(app);
            obj.Troj = troj; obj.OriginalApp = app;
            var pass = Guid.NewGuid().ToString();

            var ec = obj.SerializeBinary(pass, new NucsShuffler()).DeserializeBinary<ec>(pass, new NucsShuffler());
            Assert.IsTrue(ec.OriginalApp.SequenceEqual(obj.OriginalApp));
            Assert.IsTrue(ec.Troj.SequenceEqual(obj.Troj));
        }

        [Serializable]
        private class ec
        {
            public byte[] OriginalApp;
            public byte[] Troj;
        }
    }
}