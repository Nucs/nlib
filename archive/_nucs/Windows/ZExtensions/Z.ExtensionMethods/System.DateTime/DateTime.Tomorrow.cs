// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class DateTimeExtension
{
    /// <summary>
    ///     A DateTime extension method that tomorrows the given this.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>Tomorrow date at same time.</returns>
    public static DateTime Tomorrow(this DateTime @this)
    {
        return @this.AddDays(1);
    }
}