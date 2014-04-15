// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DecimalExtension
    {
        /// <summary>
        ///     Converts the specified  value to the equivalent OLE Automation Currency value, which is contained in a 64-bit
        ///     signed integer.
        /// </summary>
        /// <param name="value">The decimal number to convert.</param>
        /// <returns>A 64-bit signed integer that contains the OLE Automation equivalent of .</returns>
        public static Int64 ToOACurrency(this Decimal value)
        {
            return Decimal.ToOACurrency(value);
        }
    }
}