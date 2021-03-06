// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DoubleExtension
{
    /// <summary>
    ///     Returns a  that represents a specified number of hours, where the specification is accurate to the nearest
    ///     millisecond.
    /// </summary>
    /// <param name="value">A number of hours accurate to the nearest millisecond.</param>
    /// <returns>An object that represents .</returns>
    public static TimeSpan FromHours(this Double value)
    {
        return TimeSpan.FromHours(value);
    }
}