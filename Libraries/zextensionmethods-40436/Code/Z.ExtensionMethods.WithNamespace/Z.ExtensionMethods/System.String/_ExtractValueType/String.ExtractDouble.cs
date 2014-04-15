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
        ///     A string extension method that extracts the Double from the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Double.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ExtractDouble
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractDouble()
        ///                   {
        ///                       // Type
        ///           
        ///                       // Exemples
        ///                       double result1 = &quot;Fizz 123 Buzz&quot;.ExtractDouble(); // return 123;
        ///                       double result2 = &quot;Fizz -123 Buzz&quot;.ExtractDouble(); // return -123;
        ///                       double result3 = &quot;-Fizz 123 Buzz&quot;.ExtractDouble(); // return 123;
        ///                       double result4 = &quot;Fizz 123.456 Buzz&quot;.ExtractDouble(); // return 123.456;
        ///                       double result5 = &quot;Fizz -123Fizz.Buzz456 Buzz&quot;.ExtractDouble(); // return -123.456;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(123, result1);
        ///                       Assert.AreEqual(-123, result2);
        ///                       Assert.AreEqual(123, result3);
        ///                       Assert.AreEqual(123.456, result4);
        ///                       Assert.AreEqual(-123.456, result5);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static double ExtractDouble(this string @this)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < @this.Length; i++)
            {
                if (Char.IsDigit(@this[i]) || @this[i] == '.')
                {
                    if (sb.Length == 0 && i > 0 && @this[i - 1] == '-')
                    {
                        sb.Append('-');
                    }
                    sb.Append(@this[i]);
                }
            }

            return Convert.ToDouble(sb.ToString());
        }
    }
}