// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;
using System.Text.RegularExpressions;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that extracts all UInt64 from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>All extracted UInt64.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ExtractManyUInt64
    ///               {
    ///                   [TestMethod]
    ///                   public void ExtractManyUInt64()
    ///                   {
    ///                       // Type
    ///           
    ///                       // Exemples
    ///                       ulong[] result1 = &quot;1Fizz-2Buzz&quot;.ExtractManyUInt64(); // return new [] {1, 2};
    ///                       ulong[] result2 = &quot;12.34Fizz-0.456&quot;.ExtractManyUInt64(); // return new [] {12, 34, 0, 456};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual((UInt64) 1, result1[0]);
    ///                       Assert.AreEqual((UInt64) 2, result1[1]);
    ///                       Assert.AreEqual((UInt64) 12, result2[0]);
    ///                       Assert.AreEqual((UInt64) 34, result2[1]);
    ///                       Assert.AreEqual((UInt64) 0, result2[2]);
    ///                       Assert.AreEqual((UInt64) 456, result2[3]);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static ulong[] ExtractManyUInt64(this string @this)
    {
        return Regex.Matches(@this, @"\d+")
                    .Cast<Match>()
                    .Select(x => Convert.ToUInt64(x.Value))
                    .ToArray();
    }
}