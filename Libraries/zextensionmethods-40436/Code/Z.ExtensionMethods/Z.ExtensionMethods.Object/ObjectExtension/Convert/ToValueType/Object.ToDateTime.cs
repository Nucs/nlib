// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts the @this to a date time.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a DateTime.</returns>
    public static DateTime ToDateTime(this object @this)
    {
        return Convert.ToDateTime(@this);
    }
}