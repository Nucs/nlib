// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DateTimeExtension
{
    /// <summary>
    ///     A DateTime extension method that query if 'time' is time equal.
    /// </summary>
    /// <param name="time">The time to act on.</param>
    /// <param name="timeToCompare">Date/Time of the time to compare.</param>
    /// <returns>true if time equal, false if not.</returns>
    public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
    {
        return (time.TimeOfDay == timeToCompare.TimeOfDay);
    }
}