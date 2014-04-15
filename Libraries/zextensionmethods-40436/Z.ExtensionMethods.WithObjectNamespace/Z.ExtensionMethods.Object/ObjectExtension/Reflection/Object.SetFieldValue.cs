// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Reflection;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that sets field value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_SetFieldValue
        ///               {
        ///                   [TestMethod]
        ///                   public void SetFieldValue()
        ///                   {
        ///                       // Type
        ///                       var @this = new TestClass();
        ///           
        ///                       // Exemples
        ///                       @this.SetFieldValue(&quot;PublicField&quot;, 1);
        ///                       @this.SetFieldValue(&quot;InternaStaticlField&quot;, 2);
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, @this.PublicField);
        ///                       Assert.AreEqual(2, TestClass.InternaStaticlField);
        ///                   }
        ///           
        ///                   public class TestClass
        ///                   {
        ///                       internal static int InternaStaticlField = 2;
        ///                       public int PublicField = 1;
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void SetFieldValue<T>(this T @this, string fieldName, object value)
        {
            Type type = @this.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            field.SetValue(@this, value);
        }
    }
}