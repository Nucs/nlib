// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).


public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that null if equals.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="value">The value.</param>
    /// <returns>A T.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_NullIfEquals
    ///               {
    ///                   [TestMethod]
    ///                   public void NullIfEquals()
    ///                   {
    ///                       // Type
    ///                       object @this = &quot;1&quot;;
    ///           
    ///                       // Exemples
    ///                       object result1 = @this.NullIfEquals(&quot;1&quot;); // return null;
    ///                       object result2 = @this.NullIfEquals(&quot;2&quot;); // return &quot;1&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(null, result1);
    ///                       Assert.AreEqual(&quot;1&quot;, result2);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T NullIfEquals<T>(this T @this, T value) where T : class
    {
        if (@this.Equals(value))
        {
            return null;
        }
        return @this;
    }
}