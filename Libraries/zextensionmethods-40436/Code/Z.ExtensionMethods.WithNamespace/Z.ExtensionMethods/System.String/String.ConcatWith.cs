// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that concatenate with.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>A string.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ConcatWith
        ///               {
        ///                   [TestMethod]
        ///                   public void ConcatWith()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;Fizz&quot;;
        ///           
        ///                       // Exemples
        ///                       string result = @this.ConcatWith(&quot;Buzz&quot;, &quot;FizzBuzz&quot;); // return &quot;FizzBuzzFizzBuzz&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;FizzBuzzFizzBuzz&quot;, result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string ConcatWith(this string @this, params string[] values)
        {
            return string.Concat(@this, string.Concat(values));
        }
    }
}