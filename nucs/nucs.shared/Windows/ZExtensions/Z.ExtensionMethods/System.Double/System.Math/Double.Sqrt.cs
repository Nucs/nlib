// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DoubleExtension
{
    /// <summary>
    ///     Returns the square root of a specified number.
    /// </summary>
    /// <param name="d">The number whose square root is to be found.</param>
    /// <returns>
    ///     One of the values in the following table.  parameter Return value Zero or positive The positive square root
    ///     of . Negative Equals Equals.
    /// </returns>
    public static Double Sqrt(this Double d)
    {
        return Math.Sqrt(d);
    }
}