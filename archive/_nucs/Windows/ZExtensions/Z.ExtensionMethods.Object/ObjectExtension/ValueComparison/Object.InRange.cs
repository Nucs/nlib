// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    /// ###
    /// <summary>Generic type extension.</summary>
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_InRange
        ///               {
        ///                   [TestMethod]
        ///                   public void InRange()
        ///                   {
        ///                       // Type
        ///                       int @this = 3;
        ///           
        ///                       // Examples
        ///                       bool value1 = @this.InRange(-4, 3); // return true;
        ///                       bool value2 = @this.InRange(10, 100); // return false;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsTrue(value1);
        ///                       Assert.IsFalse(value2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static bool InRange<T>(this T @this, T minValue, T maxValue) where T : IComparable<T>
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }
    }
}