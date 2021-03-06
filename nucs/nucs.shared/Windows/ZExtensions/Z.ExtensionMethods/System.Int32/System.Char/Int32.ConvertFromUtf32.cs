// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class Int32Extension
{
    /// <summary>
    ///     Converts the specified Unicode code point into a UTF-16 encoded string.
    /// </summary>
    /// <param name="utf32">A 21-bit Unicode code point.</param>
    /// <returns>
    ///     A string consisting of one  object or a surrogate pair of  objects equivalent to the code point specified by
    ///     the  parameter.
    /// </returns>
    public static String ConvertFromUtf32(this Int32 utf32)
    {
        return Char.ConvertFromUtf32(utf32);
    }
}