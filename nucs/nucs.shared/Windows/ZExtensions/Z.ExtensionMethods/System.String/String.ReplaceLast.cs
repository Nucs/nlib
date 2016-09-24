// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Linq;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that replace last occurence.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The string with the last occurence of old value replace by new value.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ReplaceLast
    ///               {
    ///                   [TestMethod]
    ///                   public void ReplaceLast()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;zzzzz&quot;;
    ///           
    ///                       // Exemples
    ///                       string result1 = @this.ReplaceLast(&quot;z&quot;, &quot;a&quot;); // return &quot;zzzza&quot;;
    ///                       string result2 = @this.ReplaceLast(3, &quot;z&quot;, &quot;a&quot;); // return &quot;zzaaa&quot;;
    ///                       string result3 = @this.ReplaceLast(3, &quot;z&quot;, &quot;za&quot;); // return &quot;zzzazaza&quot;;
    ///                       string result4 = @this.ReplaceLast(4, &quot;z&quot;, &quot;a&quot;); // return &quot;zaaaa&quot;;
    ///                       string result5 = @this.ReplaceLast(5, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
    ///                       string result6 = @this.ReplaceLast(10, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;zzzza&quot;, result1);
    ///                       Assert.AreEqual(&quot;zzaaa&quot;, result2);
    ///                       Assert.AreEqual(&quot;zzzazaza&quot;, result3);
    ///                       Assert.AreEqual(&quot;zaaaa&quot;, result4);
    ///                       Assert.AreEqual(&quot;aaaaa&quot;, result5);
    ///                       Assert.AreEqual(&quot;aaaaa&quot;, result6);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ReplaceLast(this string @this, string oldValue, string newValue)
    {
        int startindex = @this.LastIndexOf(oldValue);

        if (startindex == -1)
        {
            return @this;
        }

        return @this.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
    }

    /// <summary>
    ///     A string extension method that replace last numbers occurences.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="number">Number of.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>The string with the last numbers occurences of old value replace by new value.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_ReplaceLast
    ///               {
    ///                   [TestMethod]
    ///                   public void ReplaceLast()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;zzzzz&quot;;
    ///           
    ///                       // Exemples
    ///                       string result1 = @this.ReplaceLast(&quot;z&quot;, &quot;a&quot;); // return &quot;zzzza&quot;;
    ///                       string result2 = @this.ReplaceLast(3, &quot;z&quot;, &quot;a&quot;); // return &quot;zzaaa&quot;;
    ///                       string result3 = @this.ReplaceLast(3, &quot;z&quot;, &quot;za&quot;); // return &quot;zzzazaza&quot;;
    ///                       string result4 = @this.ReplaceLast(4, &quot;z&quot;, &quot;a&quot;); // return &quot;zaaaa&quot;;
    ///                       string result5 = @this.ReplaceLast(5, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
    ///                       string result6 = @this.ReplaceLast(10, &quot;z&quot;, &quot;a&quot;); // return &quot;aaaaa&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;zzzza&quot;, result1);
    ///                       Assert.AreEqual(&quot;zzaaa&quot;, result2);
    ///                       Assert.AreEqual(&quot;zzzazaza&quot;, result3);
    ///                       Assert.AreEqual(&quot;zaaaa&quot;, result4);
    ///                       Assert.AreEqual(&quot;aaaaa&quot;, result5);
    ///                       Assert.AreEqual(&quot;aaaaa&quot;, result6);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ReplaceLast(this string @this, int number, string oldValue, string newValue)
    {
        List<string> list = @this.Split(oldValue).ToList();
        int old = Math.Max(0, list.Count - number - 1);
        IEnumerable<string> listStart = list.Take(old);
        IEnumerable<string> listEnd = list.Skip(old);

        return string.Join(oldValue, listStart.ToArray()) +
               (old > 0 ? oldValue : "") +
               string.Join(newValue, listEnd.ToArray());
    }
}