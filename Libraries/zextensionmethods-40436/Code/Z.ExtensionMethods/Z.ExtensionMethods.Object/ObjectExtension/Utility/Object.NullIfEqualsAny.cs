// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that null if equals any.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="values">A variable-length parameters list containing values.</param>
    /// <returns>A T.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_NullIfEqualsAny
    ///               {
    ///                   [TestMethod]
    ///                   public void NullIfEqualsAny()
    ///                   {
    ///                       // Type
    ///                       object @this = &quot;1&quot;;
    ///           
    ///                       // Exemples
    ///                       object result1 = @this.NullIfEqualsAny(&quot;0&quot;, &quot;1&quot;, &quot;2&quot;); // return null;
    ///                       object result2 = @this.NullIfEqualsAny(&quot;2&quot;); // return &quot;1&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(null, result1);
    ///                       Assert.AreEqual(&quot;1&quot;, result2);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T NullIfEqualsAny<T>(this T @this, params T[] values) where T : class
    {
        if (Array.IndexOf(values, @this) != -1)
        {
            return null;
        }
        return @this;
    }
}