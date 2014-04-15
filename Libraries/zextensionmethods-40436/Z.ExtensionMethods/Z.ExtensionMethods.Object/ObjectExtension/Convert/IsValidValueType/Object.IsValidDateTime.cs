// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that query if '@this' is valid System.DateTime.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if valid System.DateTime, false if not.</returns>
    public static bool IsValidDateTime(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        DateTime result;
        return DateTime.TryParse(@this.ToString(), out result);
    }
}