using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_DataTable_ToExpandoObjects
    {
        [TestMethod]
        public void ToExpandoObjects()
        {
            // Type
            var @this = new DataTable();

            // Variables
            @this.Columns.Add("IntColumn", typeof (int));
            @this.Columns.Add("StringColumn", typeof (string));
            @this.Rows.Add(1, "Fizz");
            @this.Rows.Add(2, "Buzz");

            // Exemples
            List<dynamic> expandoObjects = @this.ToExpandoObjects().ToList();

            // Unit Test
            Assert.AreEqual(2, expandoObjects.Count);
            Assert.AreEqual(1, expandoObjects[0].IntColumn);
            Assert.AreEqual("Fizz", expandoObjects[0].StringColumn);
            Assert.AreEqual(2, expandoObjects[1].IntColumn);
            Assert.AreEqual("Buzz", expandoObjects[1].StringColumn);
        }
    }
}