// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that extracts all UInt32 from the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted UInt32.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ExtractManyUInt32
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractManyUInt32()
        ///                   {
        ///                       // Type
        ///           
        ///                       // Exemples
        ///                       uint[] result1 = &quot;1Fizz-2Buzz&quot;.ExtractManyUInt32(); // return new [] {1, 2};
        ///                       uint[] result2 = &quot;12.34Fizz-0.456&quot;.ExtractManyUInt32(); // return new [] {12, 34, 0, 456};
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual((UInt32) 1, result1[0]);
        ///                       Assert.AreEqual((UInt32) 2, result1[1]);
        ///                       Assert.AreEqual((UInt32) 12, result2[0]);
        ///                       Assert.AreEqual((UInt32) 34, result2[1]);
        ///                       Assert.AreEqual((UInt32) 0, result2[2]);
        ///                       Assert.AreEqual((UInt32) 456, result2[3]);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static uint[] ExtractManyUInt32(this string @this)
        {
            return Regex.Matches(@this, @"\d+")
                        .Cast<Match>()
                        .Select(x => Convert.ToUInt32(x.Value))
                        .ToArray();
        }
    }
}