// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that query if '@this' is assignable from.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if assignable from, false if not.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IsAssignableFrom
    ///               {
    ///                   [TestMethod]
    ///                   public void IsAssignableFrom()
    ///                   {
    ///                       // Type
    ///                       var stringObject = (object) &quot;FizzBuzz&quot;;
    ///           
    ///                       // Exemples
    ///                       bool result1 = stringObject.IsAssignableFrom(typeof (string)); // return true;
    ///                       bool result2 = stringObject.IsAssignableFrom&lt;string&gt;(); // return true;
    ///                       bool result3 = stringObject.IsAssignableFrom&lt;object&gt;(); // return false;
    ///                       bool result4 = stringObject.IsAssignableFrom&lt;int&gt;(); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(result1);
    ///                       Assert.IsTrue(result2);
    ///                       Assert.IsFalse(result3);
    ///                       Assert.IsFalse(result4);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool IsAssignableFrom<T>(this object @this)
    {
        Type type = @this.GetType();
        return type.IsAssignableFrom(typeof (T));
    }

    /// <summary>
    ///     An object extension method that query if '@this' is assignable from.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <returns>true if assignable from, false if not.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IsAssignableFrom
    ///               {
    ///                   [TestMethod]
    ///                   public void IsAssignableFrom()
    ///                   {
    ///                       // Type
    ///                       var stringObject = (object) &quot;FizzBuzz&quot;;
    ///           
    ///                       // Exemples
    ///                       bool result1 = stringObject.IsAssignableFrom(typeof (string)); // return true;
    ///                       bool result2 = stringObject.IsAssignableFrom&lt;string&gt;(); // return true;
    ///                       bool result3 = stringObject.IsAssignableFrom&lt;object&gt;(); // return false;
    ///                       bool result4 = stringObject.IsAssignableFrom&lt;int&gt;(); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(result1);
    ///                       Assert.IsTrue(result2);
    ///                       Assert.IsFalse(result3);
    ///                       Assert.IsFalse(result4);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool IsAssignableFrom(this object @this, Type targetType)
    {
        Type type = @this.GetType();
        return type.IsAssignableFrom(targetType);
    }
}