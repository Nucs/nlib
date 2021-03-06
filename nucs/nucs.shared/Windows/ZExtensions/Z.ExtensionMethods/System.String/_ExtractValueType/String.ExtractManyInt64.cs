// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;
using System.Text.RegularExpressions;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that extracts all Int64 from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>All extracted Int64.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ExtractManyInt64
    ///               {
    ///                   [TestMethod]
    ///                   public void ExtractManyInt64()
    ///                   {
    ///                       // Type
    ///           
    ///                       // Exemples
    ///                       long[] result1 = &quot;1Fizz-2Buzz&quot;.ExtractManyInt64(); // return new [] {1, -2};
    ///                       long[] result2 = &quot;12.34Fizz-0.456&quot;.ExtractManyInt64(); // return new [] {12, 34, 0, 456};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(1, result1[0]);
    ///                       Assert.AreEqual(-2, result1[1]);
    ///                       Assert.AreEqual(12, result2[0]);
    ///                       Assert.AreEqual(34, result2[1]);
    ///                       Assert.AreEqual(0, result2[2]);
    ///                       Assert.AreEqual(456, result2[3]);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static long[] ExtractManyInt64(this string @this)
    {
        return Regex.Matches(@this, @"[-]?\d+")
                    .Cast<Match>()
                    .Select(x => Convert.ToInt64(x.Value))
                    .ToArray();
    }
}