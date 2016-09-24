// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Reflection;

namespace Z.ExtensionMethods.Object.Reflection
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that gets a field value (Public | NonPublic | Instance | Static)
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The field value.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_GetFieldValue
        ///               {
        ///                   [TestMethod]
        ///                   public void GetFieldValue()
        ///                   {
        ///                       // Type
        ///                       var @this = new TestClass();
        ///           
        ///                       // Exemples
        ///                       object result1 = @this.GetFieldValue(&quot;PublicField&quot;); // return 1;
        ///                       object result2 = @this.GetFieldValue(&quot;InternaStaticlField&quot;); // return 2;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(2, result2);
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
        public static object GetFieldValue<T>(this T @this, string fieldName)
        {
            Type type = @this.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return field.GetValue(@this);
        }
    }
}