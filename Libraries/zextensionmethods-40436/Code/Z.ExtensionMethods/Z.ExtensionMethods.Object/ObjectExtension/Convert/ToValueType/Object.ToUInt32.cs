// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts the @this to an u int 32.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an uint.</returns>
    public static uint ToUInt32(this object @this)
    {
        return Convert.ToUInt32(@this);
    }
}