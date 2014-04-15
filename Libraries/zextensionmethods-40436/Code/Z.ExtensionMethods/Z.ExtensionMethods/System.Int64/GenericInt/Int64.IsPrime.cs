// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

/// ###
/// <summary>Int 64 extension.</summary>
public static partial class Int64Extension
{
    /// <summary>
    ///     An Int64 extension method that query if '@this' is prime.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if prime, false if not.</returns>
    public static bool IsPrime(this Int64 @this)
    {
        if (@this == 1 || @this == 2)
        {
            return true;
        }

        if (@this%2 == 0)
        {
            return false;
        }

        var sqrt = (Int64) Math.Sqrt(@this);
        for (Int64 t = 3; t <= sqrt; t = t + 2)
        {
            if (@this%t == 0)
            {
                return false;
            }
        }

        return true;
    }
}