using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nucs.Serialization.Tests {
    [TestClass]
    public class BinaryTests {
        [TestMethod]
        public void SerializeTest() {
            var obj = new ec();
            var rand = new Random();
            var troj = new byte[rand.Next(10000, 128000)]; rand.NextBytes(troj);
            var app = new byte[rand.Next(10000, 128000)]; rand.NextBytes(app);
            obj.Troj = troj; obj.OriginalApp = app;

            var ec = obj.SerializeBinary().DeserializeBinary<ec>();

            Assert.IsTrue(ec.OriginalApp.SequenceEqual(obj.OriginalApp));
            Assert.IsTrue(ec.Troj.SequenceEqual(obj.Troj));
        }
        
        [Serializable]
        private class ec {
            public byte[] OriginalApp;
            public byte[] Troj;
        }
    }
}