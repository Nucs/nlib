using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    public enum TestEnum
    {
        [System.ComponentModel.Description("Test Description")] Test
    }

    [TestClass]
    public class System_Object_GetCustomAttributeDescription
    {
        /// <summary>
        ///     System.String GetCustomAttributeDescription(System.Object)
        /// </summary>
        [TestMethod]
        public void GetCustomAttributeDescription()
        {
            // Type
            var @this = TestEnum.Test;

            // Exemples
            string result = @this.GetCustomAttributeDescription(); // return "Test Description";

            // Unit Test
            Assert.AreEqual("Test Description", result);
        }
    }
}