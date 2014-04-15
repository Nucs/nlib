// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="x">A double-precision floating-point number to be raised to a power.</param>
        /// <param name="y">A double-precision floating-point number that specifies a power.</param>
        /// <returns>The number  raised to the power .</returns>
        public static Double Pow(this Double x, Double y)
        {
            return Math.Pow(x, y);
        }
    }
}