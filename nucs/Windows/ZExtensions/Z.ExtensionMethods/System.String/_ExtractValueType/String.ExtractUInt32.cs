// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Text;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that extracts the UInt32 from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The extracted UInt32.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ExtractUInt32
    ///               {
    ///                   [TestMethod]
    ///                   public void ExtractUInt32()
    ///                   {
    ///                       // Type
    ///           
    ///                       // Exemples
    ///                       uint result1 = &quot;Fizz 123 Buzz&quot;.ExtractUInt32(); // return 123;
    ///                       uint result2 = &quot;Fizz -123 Buzz&quot;.ExtractUInt32(); // return 123;
    ///                       uint result3 = &quot;-Fizz 123 Buzz&quot;.ExtractUInt32(); // return 123;
    ///                       uint result4 = &quot;Fizz 123.456 Buzz&quot;.ExtractUInt32(); // return 123456;
    ///                       uint result5 = &quot;Fizz -123Fizz.Buzz456 Buzz&quot;.ExtractUInt32(); // return 123456;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual((UInt32) 123, result1);
    ///                       Assert.AreEqual((UInt32) 123, result2);
    ///                       Assert.AreEqual((UInt32) 123, result3);
    ///                       Assert.AreEqual((UInt32) 123456, result4);
    ///                       Assert.AreEqual((UInt32) 123456, result5);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static uint ExtractUInt32(this string @this)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < @this.Length; i++)
        {
            if (Char.IsDigit(@this[i]))
            {
                sb.Append(@this[i]);
            }
        }

        return Convert.ToUInt32(sb.ToString());
    }
}