// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DecimalExtension
    {
        /// <summary>
        ///     Converts the value of the specified  to the equivalent 32-bit signed integer.
        /// </summary>
        /// <param name="d">The decimal number to convert.</param>
        /// <returns>A 32-bit signed integer equivalent to the value of .</returns>
        public static Int32 ToInt32(this Decimal d)
        {
            return Decimal.ToInt32(d);
        }
    }
}