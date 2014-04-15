using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_DecryptRSA
    {
        [TestMethod]
        public void DecryptRSA()
        {
            // Type
            string @this = "7E-24-A5-CF-8E-7A-83-52-90-CA-81-6F-26-04-7C-E6-F4-25-47-26-A9-55-04-83-32-78-1A-C0-E4-5D-90-66-A6-E1-58-59-A8-48-E1-20-21-B9-FE-84-31-53-52-9B-45-E1-B2-93-71-92-DA-29-5B-99-D1-41-19-9C-3E-13-4B-2B-BC-08-94-31-A4-F9-B9-0A-04-2F-C6-78-B1-47-27-11-2C-E6-AF-BF-A8-F2-F3-F6-4E-CB-EB-79-5E-80-C6-A1-0A-D7-7C-F1-16-0E-41-14-4E-76-7E-9E-DD-61-BF-11-5E-62-79-2D-C4-11-D2-F6-3D-7F-DD-87-C4-4E";

            // Examples
            string value = @this.DecryptRSA("Buzz"); // return "Fizz";

            // Unit Test
            Assert.AreEqual("Fizz", value);
        }
    }
}