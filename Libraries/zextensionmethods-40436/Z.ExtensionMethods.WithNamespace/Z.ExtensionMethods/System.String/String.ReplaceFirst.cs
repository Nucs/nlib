// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Linq;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that replace first occurence.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the first occurence of old value replace by new value.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ReplaceFirst
        ///               {
        ///                   [TestMethod]
        ///                   public void ReplaceFirst()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;zzzzz&quot;;
        ///           
        ///                       // Exemples
        ///                       string result1 = @this.ReplaceFirst(&quot;z&quot;, &quot;a&quot;); // return &quot;azzzz&quot;;
        ///                       string result2 = @this.ReplaceFirst(3, &quot;z&quot;, &quot;a&quot;); // return &quot;aaazz&quot;;
        ///                       string result3 = @this.ReplaceFirst(3, &quot;z&quot;, &quot;za&quot;); // return &quot;zazazazz&quot;;
        ///                       string result4 = @this.ReplaceFirst(4, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaz&quot;;
        ///                       string result5 = @this.ReplaceFirst(5, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
        ///                       string result6 = @this.ReplaceFirst(10, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;azzzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;aaazz&quot;, result2);
        ///                       Assert.AreEqual(&quot;zazazazz&quot;, result3);
        ///                       Assert.AreEqual(&quot;aaaaz&quot;, result4);
        ///                       Assert.AreEqual(&quot;aaaaa&quot;, result5);
        ///                       Assert.AreEqual(&quot;aaaaa&quot;, result6);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string ReplaceFirst(this string @this, string oldValue, string newValue)
        {
            int startindex = @this.IndexOf(oldValue);

            if (startindex == -1)
            {
                return @this;
            }

            return @this.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
        }

        /// <summary>
        ///     A string extension method that replace first number of occurences.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="number">Number of.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the numbers of occurences of old value replace by new value.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_ReplaceFirst
        ///               {
        ///                   [TestMethod]
        ///                   public void ReplaceFirst()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;zzzzz&quot;;
        ///           
        ///                       // Exemples
        ///                       string result1 = @this.ReplaceFirst(&quot;z&quot;, &quot;a&quot;); // return &quot;azzzz&quot;;
        ///                       string result2 = @this.ReplaceFirst(3, &quot;z&quot;, &quot;a&quot;); // return &quot;aaazz&quot;;
        ///                       string result3 = @this.ReplaceFirst(3, &quot;z&quot;, &quot;za&quot;); // return &quot;zazazazz&quot;;
        ///                       string result4 = @this.ReplaceFirst(4, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaz&quot;;
        ///                       string result5 = @this.ReplaceFirst(5, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
        ///                       string result6 = @this.ReplaceFirst(10, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;azzzz&quot;, result1);
        ///                       Assert.AreEqual(&quot;aaazz&quot;, result2);
        ///                       Assert.AreEqual(&quot;zazazazz&quot;, result3);
        ///                       Assert.AreEqual(&quot;aaaaz&quot;, result4);
        ///                       Assert.AreEqual(&quot;aaaaa&quot;, result5);
        ///                       Assert.AreEqual(&quot;aaaaa&quot;, result6);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string ReplaceFirst(this string @this, int number, string oldValue, string newValue)
        {
            List<string> list = @this.Split(oldValue).ToList();
            int old = number + 1;
            IEnumerable<string> listStart = list.Take(old);
            IEnumerable<string> listEnd = list.Skip(old);

            return string.Join(newValue, listStart) +
                   (listEnd.Any() ? oldValue : "") +
                   string.Join(oldValue, listEnd);
        }
    }
}