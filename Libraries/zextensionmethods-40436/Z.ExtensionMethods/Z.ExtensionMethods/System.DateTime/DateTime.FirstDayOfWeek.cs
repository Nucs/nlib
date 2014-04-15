// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DateTimeExtension
{
    /// <summary>
    ///     A DateTime extension method that first day of week.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A DateTime.</returns>
    public static DateTime FirstDayOfWeek(this DateTime @this)
    {
        return new DateTime(@this.Year, @this.Month, @this.Day).AddDays(-(int) @this.DayOfWeek);
    }
}