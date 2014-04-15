// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Reflection;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that sets property value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_SetPropertyValue
    ///               {
    ///                   [TestMethod]
    ///                   public void SetPropertyValue()
    ///                   {
    ///                       // Type
    ///                       var @this = new TestClass();
    ///           
    ///                       // Exemples
    ///                       @this.SetPropertyValue(&quot;PublicProperty&quot;, 1);
    ///                       @this.SetPropertyValue(&quot;InternaStaticlProperty&quot;, 2);
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(1, @this.PublicProperty);
    ///                       Assert.AreEqual(2, TestClass.InternaStaticlProperty);
    ///                   }
    ///           
    ///                   public class TestClass
    ///                   {
    ///                       internal static int InternaStaticlProperty { get; set; }
    ///                       public int PublicProperty { get; set; }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void SetPropertyValue<T>(this T @this, string propertyName, object value)
    {
        Type type = @this.GetType();
        PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        property.SetValue(@this, value, null);
    }
}