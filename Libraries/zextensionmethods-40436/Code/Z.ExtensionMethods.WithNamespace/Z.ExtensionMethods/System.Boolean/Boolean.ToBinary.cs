// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class BooleanExtension
    {
        /// <summary>
        ///     A bool extension method that convert this object into a binary representation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A binary represenation of this object.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Boolean_ToBinary
        ///               {
        ///                   [TestMethod]
        ///                   public void ToBinary()
        ///                   {
        ///                       // Type
        ///                       bool @thisTrue = true;
        ///                       bool @thisFalse = false;
        ///           
        ///                       // Exemples
        ///                       byte result1 = @thisTrue.ToBinary(); // return 1;
        ///                       byte result2 = @thisFalse.ToBinary(); // return 0;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static byte ToBinary(this bool @this)
        {
            return Convert.ToByte(@this);
        }
    }
}