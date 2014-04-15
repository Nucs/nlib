// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that removes the letter.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A string.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_RemoveWhere
        ///               {
        ///                   [TestMethod]
        ///                   public void RemoveWhere()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;Fizz1Buzz2&quot;;
        ///           
        ///                       // Exemples
        ///                       string result = @this.RemoveWhere(c =&gt; c.IsNumber()); // return &quot;FizzBuzz&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string RemoveWhere(this string @this, Func<char, bool> predicate)
        {
            return new string(@this.ToCharArray().Where(x => !predicate(x)).ToArray());
        }
    }
}