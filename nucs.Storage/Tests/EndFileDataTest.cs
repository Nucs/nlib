using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TFer.EndData.Tests {
    [TestClass]
    public class EndFileDataTest {
        
        [TestMethod]
        public void WriteToEmptyFileTest() {
            var p = TestHelper.GetTemp();
            var obj = EndDataStorageTestClass.Fetch();
            try { 
                p.WriteEndData(obj);
                Assert.IsTrue(EndDataStorageTestClass.Verify(p.ReadEndData<EndDataStorageTestClass>().Result));
            } finally {
                p.Delete();
            }
        }


    }
}