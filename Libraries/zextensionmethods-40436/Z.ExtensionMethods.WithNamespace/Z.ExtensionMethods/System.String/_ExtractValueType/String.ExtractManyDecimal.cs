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
        ///     A string extension method that extracts all Decimal from the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>All extracted Decimal.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ExtractManyDecimal
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractManyDecimal()
        ///                   {
        ///                       // Type
        ///           
        ///                       // Exemples
        ///                       decimal[] result1 = &quot;1Fizz-2Buzz&quot;.ExtractManyDecimal(); // return new [] {1, -2};
        ///                       decimal[] result2 = &quot;12.34Fizz-0.456&quot;.ExtractManyDecimal(); // return new [] {12.34, -0.456};
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(1M, result1[0]);
        ///                       Assert.AreEqual(-2M, result1[1]);
        ///                       Assert.AreEqual(12.34M, result2[0]);
        ///                       Assert.AreEqual(-0.456M, result2[1]);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static decimal[] ExtractManyDecimal(this string @this)
        {
            return Regex.Matches(@this, @"[-]?\d+(\.\d+)?")
                        .Cast<Match>()
                        .Select(x => Convert.ToDecimal(x.Value))
                        .ToArray();
        }
    }
}