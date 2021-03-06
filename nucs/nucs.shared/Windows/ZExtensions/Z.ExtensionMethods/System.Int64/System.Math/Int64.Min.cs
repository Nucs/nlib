// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class Int64Extension
{
    /// <summary>
    ///     Returns the smaller of two 64-bit signed integers.
    /// </summary>
    /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
    /// <returns>Parameter  or , whichever is smaller.</returns>
    public static Int64 Min(this Int64 val1, Int64 val2)
    {
        return Math.Min(val1, val2);
    }
}