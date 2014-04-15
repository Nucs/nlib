// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method to determines whether the object is equal to any of the provided values.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The object to be compared.</param>
    /// <param name="values">The value list to compare with the object.</param>
    /// <returns>true if the values list contains the object, else false.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_In
    ///               {
    ///                   [TestMethod]
    ///                   public void In()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;a&quot;;
    ///           
    ///                       // Examples
    ///                       bool value1 = @this.In(&quot;a&quot;, &quot;1&quot;, &quot;2&quot;, &quot;3&quot;); // return true;
    ///                       bool value2 = @this.In(&quot;1&quot;, &quot;2&quot;, &quot;3&quot;); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(value1);
    ///                       Assert.IsFalse(value2);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool In<T>(this T @this, params T[] values)
    {
        return Array.IndexOf(values, @this) != -1;
    }
}