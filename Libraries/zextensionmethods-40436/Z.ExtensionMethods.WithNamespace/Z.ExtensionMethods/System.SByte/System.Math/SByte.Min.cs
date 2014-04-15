// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class SByteExtension
    {
        /// <summary>
        ///     Returns the smaller of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static SByte Min(this SByte val1, SByte val2)
        {
            return Math.Min(val1, val2);
        }
    }
}