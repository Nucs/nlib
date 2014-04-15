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
        ///     A string extension method that extracts the Decimal from the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted Decimal.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ExtractDecimal
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractDecimal()
        ///                   {
        ///                       // Type
        ///           
        ///                       // Exemples
        ///                       decimal result1 = &quot;Fizz 123 Buzz&quot;.ExtractDecimal(); // return 123;
        ///                       decimal result2 = &quot;Fizz -123 Buzz&quot;.ExtractDecimal(); // return -123;
        ///                       decimal result3 = &quot;-Fizz 123 Buzz&quot;.ExtractDecimal(); // return 123;
        ///                       decimal result4 = &quot;Fizz 123.456 Buzz&quot;.ExtractDecimal(); // return 123.456;
        ///                       decimal result5 = &quot;Fizz -123Fizz.Buzz456 Buzz&quot;.ExtractDecimal(); // return -123.456;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(123M, result1);
        ///                       Assert.AreEqual(-123M, result2);
        ///                       Assert.AreEqual(123M, result3);
        ///                       Assert.AreEqual(123.456M, result4);
        ///                       Assert.AreEqual(-123.456M, result5);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static decimal ExtractDecimal(this string @this)
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

            return Convert.ToDecimal(sb.ToString());
        }
    }
}