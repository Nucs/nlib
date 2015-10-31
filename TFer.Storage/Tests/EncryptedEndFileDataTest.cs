using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nucs.Cryptography;
using TFer.Serialization;

namespace TFer.EndData.Tests {

    [TestClass]
    public class EncryptedEndFileDataTest {
        
        [TestMethod]
        public void ToEmptyFileDefaultEncryptedTest() {
            var p = TestHelper.GetTemp();
            var obj = EndDataStorageTestClass.Fetch();
            string pass = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            try {
                p.WriteEndData(obj, pass);
                Assert.IsTrue(EndDataStorageTestClass.Verify(p.ReadEndData<EndDataStorageTestClass>(pass).Result));
            } finally {
                p.Delete();
            }
        }

        [TestMethod]
        public void ToFileNucsDefaultEncryptedTest() {
            var p = TestHelper.GetTemp();
            var obj = EndDataStorageTestClass.Fetch();
            string pass = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var rand = new Random();
            var data = new byte[rand.Next(10000, 128000)]; rand.NextBytes(data);
            using (var fs = new FileStream(p.FullName, FileMode.OpenOrCreate))
                fs.Write(data, 0, data.Length);

            try {
                p.WriteEndData(obj, pass);
                Assert.IsTrue(EndDataStorageTestClass.Verify(p.ReadEndData<EndDataStorageTestClass>(pass).Result));
            }
            finally
            {
                p.Delete();
            }
        }

        [TestMethod]
        public void ToEmptyFileNucsShuffleEncryptedTest() {
            var p = TestHelper.GetTemp();
            var obj = EndDataStorageTestClass.Fetch();
            string pass = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            try {
                p.WriteEndData(obj, pass, new NucsShuffler());
                Assert.IsTrue(EndDataStorageTestClass.Verify(p.ReadEndData<EndDataStorageTestClass>(pass, new NucsShuffler()).Result));
            } finally {
                p.Delete();
            }
        }

        [TestMethod]
        public void ToFileNucsShuffleEncryptedTest() {
            var p = TestHelper.GetTemp();
            var obj = EndDataStorageTestClass.Fetch();
            string pass = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var rand = new Random();
            var data = new byte[rand.Next(10000, 128000)]; rand.NextBytes(data);
            using (var fs = new FileStream(p.FullName, FileMode.OpenOrCreate))
                fs.Write(data, 0, data.Length);

            try {
                p.WriteEndData(obj, pass, new NucsShuffler());
                Assert.IsTrue(EndDataStorageTestClass.Verify(p.ReadEndData<EndDataStorageTestClass>(pass, new NucsShuffler()).Result));
            } finally {
                p.Delete();
            }
        }

        [TestMethod]
        public void ToFileNucsShuffleEncryptedForEndContainerTest() {
            var p = TestHelper.GetTemp();
            var obj = new ec();
            
            string pass = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var rand = new Random();
            var data = new byte[rand.Next(10000, 128000)]; rand.NextBytes(data);
            var troj = new byte[rand.Next(10000, 128000)]; rand.NextBytes(troj);
            var app = new byte[rand.Next(10000, 128000)]; rand.NextBytes(app);
            obj.Troj = troj; obj.OriginalApp = app;

            using (var fs = new FileStream(p.FullName, FileMode.OpenOrCreate))
                fs.Write(data, 0, data.Length);

            try {
                p.WriteEndData(obj, pass, new NucsShuffler());
                var dec = p.ReadEndData<ec>(pass, new NucsShuffler());
                if (dec.Failed)
                    throw dec.Exception;
                var result = dec.Result;
                Assert.IsTrue(result.OriginalApp.SequenceEqual(obj.OriginalApp));
                Assert.IsTrue(result.Troj.SequenceEqual(obj.Troj));
            } finally {
                p.Delete();
            }
        }

        [Serializable]
        public class ec {
            public byte[] OriginalApp = new byte[0];
            public byte[] Troj = new byte[0];
        }

    }
}