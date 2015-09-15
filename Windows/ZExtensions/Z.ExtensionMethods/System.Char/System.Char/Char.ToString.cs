// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class CharExtension
{
    /// <summary>
    ///     Converts the specified Unicode character to its equivalent string representation.
    /// </summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The string representation of the value of .</returns>
    public static String ToString(this Char c)
    {
        return Char.ToString(c);
    }
}