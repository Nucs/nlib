// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class Int32Extension
{
    /// <summary>
    ///     Produces the full product of two 32-bit numbers.
    /// </summary>
    /// <param name="a">The first number to multiply.</param>
    /// <param name="b">The second number to multiply.</param>
    /// <returns>The number containing the product of the specified numbers.</returns>
    public static Int64 BigMul(this Int32 a, Int32 b)
    {
        return Math.BigMul(a, b);
    }
}