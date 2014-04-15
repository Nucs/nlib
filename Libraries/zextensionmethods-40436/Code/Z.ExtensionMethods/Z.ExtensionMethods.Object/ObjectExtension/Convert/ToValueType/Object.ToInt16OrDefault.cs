// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return default(short);
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this, short defaultValue)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     An object extension method that converts this object to an int 16 or default.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The given data converted to a short.</returns>
    public static short ToInt16OrDefault(this object @this, Func<short> defaultValueFactory)
    {
        try
        {
            return Convert.ToInt16(@this);
        }
        catch (Exception)
        {
            return defaultValueFactory();
        }
    }
}