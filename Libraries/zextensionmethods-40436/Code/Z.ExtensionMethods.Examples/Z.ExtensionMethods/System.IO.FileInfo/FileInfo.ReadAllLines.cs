using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_IO_FileInfo_ReadAllLines
    {
        [TestMethod]
        public void ReadAllLines()
        {
            // Type
            var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_ReadAllLines.txt"));

            // Intialization
            using (FileStream stream = @this.Create())
            {
                byte[] byteToWrites = Encoding.Default.GetBytes("Fizz" + Environment.NewLine + "Buzz");
                stream.Write(byteToWrites, 0, byteToWrites.Length);
            }

            // Examples
            List<string> result = @this.ReadLines().ToList(); // return new [] {"Fizz", "Buzz"};

            // Unit Test
            Assert.AreEqual("Fizz", result[0]);
            Assert.AreEqual("Buzz", result[1]);
        }
    }
}