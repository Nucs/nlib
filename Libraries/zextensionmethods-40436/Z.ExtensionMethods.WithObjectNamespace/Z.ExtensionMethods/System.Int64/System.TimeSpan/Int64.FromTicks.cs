// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class Int64Extension
{
    /// <summary>
    ///     Returns a  that represents a specified time, where the specification is in units of ticks.
    /// </summary>
    /// <param name="value">A number of ticks that represent a time.</param>
    /// <returns>An object that represents .</returns>
    public static TimeSpan FromTicks(this Int64 value)
    {
        return TimeSpan.FromTicks(value);
    }
}