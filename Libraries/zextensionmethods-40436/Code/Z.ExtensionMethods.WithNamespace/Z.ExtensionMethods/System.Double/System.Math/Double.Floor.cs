// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns the largest integer less than or equal to the specified double-precision floating-point number.
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>The largest integer less than or equal to . If  is equal to , , or , that value is returned.</returns>
        public static Double Floor(this Double d)
        {
            return Math.Floor(d);
        }
    }
}