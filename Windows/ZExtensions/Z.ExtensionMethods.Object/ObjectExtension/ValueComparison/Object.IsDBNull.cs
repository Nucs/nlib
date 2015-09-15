// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     Returns an indication whether the specified object is of type .
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">An object.</param>
        /// <returns>true if  is of type ; otherwise, false.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_IsDBNull
        ///               {
        ///                   [TestMethod]
        ///                   public void IsDBNull()
        ///                   {
        ///                       // Type
        ///                       object @thisDBNull = DBNull.Value;
        ///                       object @thisNull = null;
        ///           
        ///                       // Examples
        ///                       bool result1 = @thisDBNull.IsDBNull(); // return true;
        ///                       bool result2 = @thisNull.IsDBNull(); // return false;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsTrue(result1);
        ///                       Assert.IsFalse(result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static Boolean IsDBNull<T>(this T value) where T : class
        {
            return Convert.IsDBNull(value);
        }
    }
}