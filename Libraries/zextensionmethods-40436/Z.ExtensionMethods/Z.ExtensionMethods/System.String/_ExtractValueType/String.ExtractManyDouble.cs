// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;
using System.Text.RegularExpressions;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that extracts all Double from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>All extracted Double.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ExtractManyDouble
    ///               {
    ///                   [TestMethod]
    ///                   public void ExtractManyDouble()
    ///                   {
    ///                       // Type
    ///           
    ///                       // Exemples
    ///                       double[] result1 = &quot;1Fizz-2Buzz&quot;.ExtractManyDouble(); // return new [] {1, -2};
    ///                       double[] result2 = &quot;12.34Fizz-0.456&quot;.ExtractManyDouble(); // return new [] {12.34, -0.456};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(1, result1[0]);
    ///                       Assert.AreEqual(-2, result1[1]);
    ///                       Assert.AreEqual(12.34, result2[0]);
    ///                       Assert.AreEqual(-0.456, result2[1]);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static double[] ExtractManyDouble(this string @this)
    {
        return Regex.Matches(@this, @"[-]?\d+(\.\d+)?")
                    .Cast<Match>()
                    .Select(x => Convert.ToDouble(x.Value))
                    .ToArray();
    }
}