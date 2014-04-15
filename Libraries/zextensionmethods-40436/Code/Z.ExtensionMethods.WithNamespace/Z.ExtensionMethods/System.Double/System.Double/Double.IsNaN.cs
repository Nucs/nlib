// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns a value that indicates whether the specified value is not a number ().
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>true if  evaluates to ; otherwise, false.</returns>
        public static Boolean IsNaN(this Double d)
        {
            return Double.IsNaN(d);
        }
    }
}