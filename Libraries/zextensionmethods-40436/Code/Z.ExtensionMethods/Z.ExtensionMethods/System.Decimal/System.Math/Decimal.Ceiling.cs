// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DecimalExtension
{
    /// <summary>
    ///     Returns the smallest integral value that is greater than or equal to the specified decimal number.
    /// </summary>
    /// <param name="d">A decimal number.</param>
    /// <returns>
    ///     The smallest integral value that is greater than or equal to . Note that this method returns a  instead of an
    ///     integral type.
    /// </returns>
    public static Decimal Ceiling(this Decimal d)
    {
        return Math.Ceiling(d);
    }
}