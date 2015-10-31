// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;
using System.Text.RegularExpressions;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that extracts all UInt16 from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>All extracted UInt16.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ExtractManyUInt16
    ///               {
    ///                   [TestMethod]
    ///                   public void ExtractManyUInt16()
    ///                   {
    ///                       // Type
    ///           
    ///                       // Exemples
    ///                       ushort[] result1 = &quot;1Fizz-2Buzz&quot;.ExtractManyUInt16(); // return new [] {1, 2};
    ///                       ushort[] result2 = &quot;12.34Fizz-0.456&quot;.ExtractManyUInt16(); // return new [] {12, 34, 0, 456};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual((UInt16) 1, result1[0]);
    ///                       Assert.AreEqual((UInt16) 2, result1[1]);
    ///                       Assert.AreEqual((UInt16) 12, result2[0]);
    ///                       Assert.AreEqual((UInt16) 34, result2[1]);
    ///                       Assert.AreEqual((UInt16) 0, result2[2]);
    ///                       Assert.AreEqual((UInt16) 456, result2[3]);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static ushort[] ExtractManyUInt16(this string @this)
    {
        return Regex.Matches(@this, @"\d+")
                    .Cast<Match>()
                    .Select(x => Convert.ToUInt16(x.Value))
                    .ToArray();
    }
}