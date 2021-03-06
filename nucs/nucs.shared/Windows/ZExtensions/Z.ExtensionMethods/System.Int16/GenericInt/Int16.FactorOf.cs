// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

/// ###
/// <summary>Int 16 extension.</summary>
public static partial class Int16Extension
{
    /// <summary>
    ///     An Int16 extension method that factor of.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="factorNumer">The factor numer.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool FactorOf(this Int16 @this, Int16 factorNumer)
    {
        return factorNumer%@this == 0;
    }
}