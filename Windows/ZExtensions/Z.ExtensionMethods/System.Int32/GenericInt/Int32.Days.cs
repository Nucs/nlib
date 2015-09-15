// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

/// ###
/// <summary>Int 32 extension.</summary>
public static partial class Int32Extension
{
    /// <summary>
    ///     An Int32 extension method that days the given this.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A TimeSpan.</returns>
    public static TimeSpan Days(this Int32 @this)
    {
        return TimeSpan.FromDays(@this);
    }
}