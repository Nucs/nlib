// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class SingleExtension
{
    /// <summary>
    ///     Returns the larger of two single-precision floating-point numbers.
    /// </summary>
    /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter  or , whichever is larger. If , or , or both  and  are equal to ,  is returned.</returns>
    public static Single Max(this Single val1, Single val2)
    {
        return Math.Max(val1, val2);
    }
}