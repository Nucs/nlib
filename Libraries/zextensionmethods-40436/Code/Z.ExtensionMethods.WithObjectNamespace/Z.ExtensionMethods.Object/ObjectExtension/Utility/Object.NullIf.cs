// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that null if.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A T.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_NullIf
        ///               {
        ///                   [TestMethod]
        ///                   public void NullIf()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;1&quot;;
        ///           
        ///                       // Exemples
        ///                       string result1 = @this.NullIf(x =&gt; x == &quot;1&quot;); // return null;
        ///                       string result2 = @this.NullIf(x =&gt; x == &quot;2&quot;); // return &quot;1&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(null, result1);
        ///                       Assert.AreEqual(&quot;1&quot;, result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T NullIf<T>(this T @this, Func<T, bool> predicate) where T : class
        {
            if (predicate(@this))
            {
                return null;
            }
            return @this;
        }
    }
}