// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that gets the first custom attribute.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="attribute">The attribute.</param>
    /// <param name="inherit">true to inherit.</param>
    /// <returns>The custom attribute.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_GetCustomAttribute
    ///               {
    ///                   [TestMethod]
    ///                   public void GetCustomAttribute()
    ///                   {
    ///                       // Type
    ///                       var @this = new TestObject();
    ///           
    ///                       // Exemples
    ///                       var result1 = @this.GetCustomAttribute&lt;DescriptionAttribute&gt;(true); // return &quot;Test Description&quot;;
    ///                       object result2 = @this.GetCustomAttribute(typeof (DescriptionAttribute), true); // return &quot;Test Description&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Test Description&quot;, result1.Description);
    ///                       Assert.AreEqual(&quot;Test Description&quot;, ((DescriptionAttribute) result2).Description);
    ///                   }
    ///           
    ///                   [Description(&quot;Test Description&quot;)]
    ///                   public class TestObject
    ///                   {
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static object GetCustomAttribute(this object @this, Type attribute, bool inherit)
    {
        return @this.GetType().GetCustomAttributes(attribute, inherit)[0];
    }

    /// <summary>
    ///     An object extension method that gets custom attribute.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="inherit">true to inherit.</param>
    /// <returns>The custom attribute.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_GetCustomAttribute
    ///               {
    ///                   [TestMethod]
    ///                   public void GetCustomAttribute()
    ///                   {
    ///                       // Type
    ///                       var @this = new TestObject();
    ///           
    ///                       // Exemples
    ///                       var result1 = @this.GetCustomAttribute&lt;DescriptionAttribute&gt;(true); // return &quot;Test Description&quot;;
    ///                       object result2 = @this.GetCustomAttribute(typeof (DescriptionAttribute), true); // return &quot;Test Description&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Test Description&quot;, result1.Description);
    ///                       Assert.AreEqual(&quot;Test Description&quot;, ((DescriptionAttribute) result2).Description);
    ///                   }
    ///           
    ///                   [Description(&quot;Test Description&quot;)]
    ///                   public class TestObject
    ///                   {
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T GetCustomAttribute<T>(this object @this, bool inherit) where T : Attribute
    {
        return (T) @this.GetType().GetCustomAttributes(typeof (T), inherit)[0];
    }
}