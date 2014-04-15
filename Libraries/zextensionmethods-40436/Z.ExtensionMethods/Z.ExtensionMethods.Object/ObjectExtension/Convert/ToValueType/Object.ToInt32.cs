// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts the @this to an int 32.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an int.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_ToInt32
    ///               {
    ///                   [TestMethod]
    ///                   public void ToInt32()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;32&quot;;
    ///           
    ///                       // Exemples
    ///                       int result = @this.ToInt32(); // return 32;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(32, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static int ToInt32(this object @this)
    {
        return Convert.ToInt32(@this);
    }
}