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
        ///     A string extension method that extracts the UInt64 from the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted UInt64.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ExtractUInt64
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractUInt64()
        ///                   {
        ///                       // Type
        ///           
        ///                       // Exemples
        ///                       ulong result1 = &quot;Fizz 123 Buzz&quot;.ExtractUInt64(); // return 123;
        ///                       ulong result2 = &quot;Fizz -123 Buzz&quot;.ExtractUInt64(); // return 123;
        ///                       ulong result3 = &quot;-Fizz 123 Buzz&quot;.ExtractUInt64(); // return 123;
        ///                       ulong result4 = &quot;Fizz 123.456 Buzz&quot;.ExtractUInt64(); // return 123456;
        ///                       ulong result5 = &quot;Fizz -123Fizz.Buzz456 Buzz&quot;.ExtractUInt64(); // return 123456;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual((UInt64) 123, result1);
        ///                       Assert.AreEqual((UInt64) 123, result2);
        ///                       Assert.AreEqual((UInt64) 123, result3);
        ///                       Assert.AreEqual((UInt64) 123456, result4);
        ///                       Assert.AreEqual((UInt64) 123456, result5);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static ulong ExtractUInt64(this string @this)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < @this.Length; i++)
            {
                if (Char.IsDigit(@this[i]))
                {
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToUInt64(sb.ToString());
        }
    }
}