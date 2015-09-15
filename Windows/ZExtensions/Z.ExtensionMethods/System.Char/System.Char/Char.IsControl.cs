// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class CharExtension
{
    /// <summary>
    ///     Indicates whether the specified Unicode character is categorized as a control character.
    /// </summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>true if  is a control character; otherwise, false.</returns>
    public static Boolean IsControl(this Char c)
    {
        return Char.IsControl(c);
    }
}