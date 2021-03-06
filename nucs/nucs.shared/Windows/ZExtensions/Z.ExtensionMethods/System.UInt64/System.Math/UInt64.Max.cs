// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class UInt64Extension
{
    /// <summary>
    ///     Returns the larger of two 64-bit unsigned integers.
    /// </summary>
    /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
    /// <returns>Parameter  or , whichever is larger.</returns>
    public static UInt64 Max(this UInt64 val1, UInt64 val2)
    {
        return Math.Max(val1, val2);
    }
}