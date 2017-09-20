// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Text;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that extracts the Int16 from the string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The extracted Int16.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ExtractInt16
    ///               {
    ///                   [TestMethod]
    ///                   public void ExtractInt16()
    ///                   {
    ///                       // Type
    ///           
    ///                       // Exemples
    ///                       short result1 = &quot;Fizz 123 Buzz&quot;.ExtractInt16(); // return 123;
    ///                       short result2 = &quot;Fizz -123 Buzz&quot;.ExtractInt16(); // return -123;
    ///                       short result3 = &quot;-Fizz 123 Buzz&quot;.ExtractInt16(); // return 123;
    ///                       short result4 = &quot;Fizz 123.4 Buzz&quot;.ExtractInt16(); // return 1234;
    ///                       short result5 = &quot;Fizz -123Fizz.Buzz4 Buzz&quot;.ExtractInt16(); // return -1234;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(123, result1);
    ///                       Assert.AreEqual(-123, result2);
    ///                       Assert.AreEqual(123, result3);
    ///                       Assert.AreEqual(1234, result4);
    ///                       Assert.AreEqual(-1234, result5);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static short ExtractInt16(this string @this)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < @this.Length; i++)
        {
            if (Char.IsDigit(@this[i]))
            {
                if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                {
                    sb.Append('-');
                }
                sb.Append(@this[i]);
            }
        }

        return Convert.ToInt16(sb.ToString());
    }
}