// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that query if '@this' is type or inherits of.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="type">The type.</param>
    /// <returns>true if type or inherits of, false if not.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IsTypeOrInheritsOf
    ///               {
    ///                   [TestMethod]
    ///                   public void IsTypeOrInheritsOf()
    ///                   {
    ///                       // Type
    ///                       var @this = new C();
    ///           
    ///                       // Exemples
    ///                       bool result1 = @this.IsTypeOrInheritsOf(typeof (C)); // return true;
    ///                       bool result2 = @this.IsTypeOrInheritsOf(typeof (B)); // return true;
    ///                       bool result3 = @this.IsTypeOrInheritsOf(typeof (A)); // return true;
    ///                       bool result4 = @this.IsTypeOrInheritsOf(typeof (string)); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(result1);
    ///                       Assert.IsTrue(result2);
    ///                       Assert.IsTrue(result3);
    ///                       Assert.IsFalse(result4);
    ///                   }
    ///           
    ///                   public class A
    ///                   {
    ///                   }
    ///           
    ///                   public class B : A
    ///                   {
    ///                   }
    ///           
    ///                   public class C : B
    ///                   {
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool IsTypeOrInheritsOf<T>(this T @this, Type type)
    {
        Type objectType = @this.GetType();

        while (true)
        {
            if (objectType.Equals(type))
            {
                return true;
            }

            if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
            {
                return false;
            }

            objectType = objectType.BaseType;
        }
    }
}