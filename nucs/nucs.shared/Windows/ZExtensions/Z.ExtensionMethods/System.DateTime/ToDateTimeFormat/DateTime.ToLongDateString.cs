// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Globalization;

/// ###
/// <summary>Date time extension.</summary>
public static partial class DateTimeExtension
{
    /// <summary>
    ///     A DateTime extension method that converts this object to a long date string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToLongDateString(this DateTime @this)
    {
        return @this.ToString("D", DateTimeFormatInfo.CurrentInfo);
    }

    /// <summary>
    ///     A DateTime extension method that converts this object to a long date string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToLongDateString(this DateTime @this, string culture)
    {
        return @this.ToString("D", new CultureInfo(culture));
    }

    /// <summary>
    ///     A DateTime extension method that converts this object to a long date string.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToLongDateString(this DateTime @this, CultureInfo culture)
    {
        return @this.ToString("D", culture);
    }
}