// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that converts the @this to a nullable int 32.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an int?</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToNullableInt32
        ///               {
        ///                   [TestMethod]
        ///                   public void ToNullableInt32()
        ///                   {
        ///                       // Type
        ///                       object @this = null;
        ///                       object @thisValue = &quot;32&quot;;
        ///           
        ///                       // Exemples
        ///                       int? result1 = @this.ToNullableInt32(); // return null;
        ///                       int? result2 = @thisValue.ToNullableInt32(); // return 32;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsFalse(result1.HasValue);
        ///                       Assert.AreEqual(32, result2.Value);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int? ToNullableInt32(this object @this)
        {
            if (@this == null || @this == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(@this);
        }
    }
}