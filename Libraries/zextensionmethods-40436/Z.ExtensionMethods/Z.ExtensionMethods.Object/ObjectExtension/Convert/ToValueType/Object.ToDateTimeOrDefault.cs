// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this)
    {
        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return default(DateTime);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this, DateTime defaultValue)
    {
        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a date time or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a DateTime.</returns>
    public static DateTime ToDateTimeOrDefault(this object @this, Func<DateTime> defaultValueFactory)
    {
        try
        {
            return Convert.ToDateTime(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
}