// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that query if '@this' is null.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null, false if not.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_IsNull
        ///               {
        ///                   [TestMethod]
        ///                   public void IsNull()
        ///                   {
        ///                       // Type
        ///                       object @thisNull = null;
        ///                       var @thisNotNull = new object();
        ///           
        ///                       // Examples
        ///                       bool value1 = @thisNull.IsNull(); // return true;
        ///                       bool value2 = @thisNotNull.IsNull(); // return false;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsTrue(value1);
        ///                       Assert.IsFalse(value2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static bool IsNull<T>(this T @this) where T : class
        {
            return @this == null;
        }
    }
}