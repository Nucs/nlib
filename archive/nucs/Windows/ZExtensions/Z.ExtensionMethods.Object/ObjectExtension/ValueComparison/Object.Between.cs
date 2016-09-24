// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that check if the value is between (exclusif) the minValue and maxValue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_Between
        ///               {
        ///                   [TestMethod]
        ///                   public void Between()
        ///                   {
        ///                       // Type
        ///                       int @this = 3;
        ///           
        ///                       // Exemples
        ///                       bool result1 = @this.Between(1, 4); // return true;
        ///                       bool result2 = @this.Between(1, 3); // return false;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsTrue(result1);
        ///                       Assert.IsFalse(result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static bool Between<T>(this T @this, T minValue, T maxValue) where T : IComparable<T>
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }
    }
}