// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Text;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that extracts the UInt16 from the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted UInt16.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ExtractUInt16
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractUInt16()
        ///                   {
        ///                       // Type
        ///           
        ///                       // Exemples
        ///                       ushort result1 = &quot;Fizz 123 Buzz&quot;.ExtractUInt16(); // return 123;
        ///                       ushort result2 = &quot;Fizz -123 Buzz&quot;.ExtractUInt16(); // return 123;
        ///                       ushort result3 = &quot;-Fizz 123 Buzz&quot;.ExtractUInt16(); // return 123;
        ///                       ushort result4 = &quot;Fizz 123.4 Buzz&quot;.ExtractUInt16(); // return 1234;
        ///                       ushort result5 = &quot;Fizz -123Fizz.Buzz4 Buzz&quot;.ExtractUInt16(); // return 1234;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual((UInt16) 123, result1);
        ///                       Assert.AreEqual((UInt16) 123, result2);
        ///                       Assert.AreEqual((UInt16) 123, result3);
        ///                       Assert.AreEqual((UInt16) 1234, result4);
        ///                       Assert.AreEqual((UInt16) 1234, result5);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static ushort ExtractUInt16(this string @this)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < @this.Length; i++)
            {
                if (Char.IsDigit(@this[i]))
                {
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToUInt16(sb.ToString());
        }
    }
}