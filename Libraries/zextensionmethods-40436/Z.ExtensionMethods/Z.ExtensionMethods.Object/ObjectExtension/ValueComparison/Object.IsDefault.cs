// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).


public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that query if 'source' is the default value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="source">The source to act on.</param>
    /// <returns>true if default, false if not.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IsDefault
    ///               {
    ///                   [TestMethod]
    ///                   public void IsDefault()
    ///                   {
    ///                       // Type
    ///                       int intDefault = 0;
    ///                       int intNotDefault = 1;
    ///           
    ///                       // Exemples
    ///                       bool result1 = intDefault.IsDefault(); // return true;
    ///                       bool result2 = intNotDefault.IsDefault(); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(result1);
    ///                       Assert.IsFalse(result2);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool IsDefault<T>(this T source)
    {
        return source.Equals(default(T));
    }
}