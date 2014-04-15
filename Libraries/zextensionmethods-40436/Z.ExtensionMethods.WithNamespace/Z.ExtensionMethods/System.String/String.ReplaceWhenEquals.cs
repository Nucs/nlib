// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that replace when equals.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The new value if the string equal old value; Otherwise old value.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ReplaceWhenEquals
        ///               {
        ///                   [TestMethod]
        ///                   public void ReplaceExact()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;Fizz&quot;;
        ///           
        ///                       // Exemples
        ///                       string result1 = @this.ReplaceWhenEquals(&quot;Fizz&quot;, &quot;Buzz&quot;); // return &quot;Buzz&quot;;
        ///                       string result2 = @this.ReplaceWhenEquals(&quot;Fiz&quot;, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string ReplaceWhenEquals(this string @this, string oldValue, string newValue)
        {
            return @this == oldValue ? newValue : @this;
        }
    }
}