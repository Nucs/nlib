// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DecimalExtension
{
    /// <summary>
    ///     Converts the value of a specified instance of  to its equivalent binary representation.
    /// </summary>
    /// <param name="d">The value to convert.</param>
    /// <returns>A 32-bit signed integer array with four elements that contain the binary representation of .</returns>
    public static Int32[] GetBits(this Decimal d)
    {
        return Decimal.GetBits(d);
    }
}