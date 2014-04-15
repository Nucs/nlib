// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this)
    {
        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return default(string);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this, string defaultValue)
    {
        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to a string or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a string.</returns>
    public static string ToStringOrDefault(this object @this, Func<string> defaultValueFactory)
    {
        try
        {
            return Convert.ToString(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
}