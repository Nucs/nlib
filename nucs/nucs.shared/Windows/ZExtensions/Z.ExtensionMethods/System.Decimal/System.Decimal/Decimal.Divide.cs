// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DecimalExtension
{
    /// <summary>
    ///     Divides two specified  values.
    /// </summary>
    /// <param name="d1">The dividend.</param>
    /// <param name="d2">The divisor.</param>
    /// <returns>The result of dividing  by .</returns>
    public static Decimal Divide(this Decimal d1, Decimal d2)
    {
        return Decimal.Divide(d1, d2);
    }
}