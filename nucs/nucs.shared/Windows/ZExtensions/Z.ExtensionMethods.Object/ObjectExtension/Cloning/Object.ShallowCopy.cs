// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Reflection;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that shallow copy.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ShallowCopy
        ///               {
        ///                   [TestMethod]
        ///                   public void ShallowCopy()
        ///                   {
        ///                       // Type
        ///                       var @this = new TestClass {Value = &quot;Fizz&quot;};
        ///           
        ///                       // Exemples
        ///                       TestClass clone = @this.ShallowCopy(); // return a shallow copy;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(@this.Value, clone.Value);
        ///                   }
        ///           
        ///                   [Serializable]
        ///                   public class TestClass
        ///                   {
        ///                       public string Value;
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T ShallowCopy<T>(this T @this)
        {
            MethodInfo method = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            return (T) method.Invoke(@this, null);
        }
    }
}