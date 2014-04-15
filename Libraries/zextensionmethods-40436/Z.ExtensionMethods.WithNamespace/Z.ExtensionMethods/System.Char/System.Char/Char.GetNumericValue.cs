// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class CharExtension
    {
        /// <summary>
        ///     Converts the specified numeric Unicode character to a double-precision floating point number.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>The numeric value of  if that character represents a number; otherwise, -1.0.</returns>
        public static Double GetNumericValue(this Char c)
        {
            return Char.GetNumericValue(c);
        }
    }
}