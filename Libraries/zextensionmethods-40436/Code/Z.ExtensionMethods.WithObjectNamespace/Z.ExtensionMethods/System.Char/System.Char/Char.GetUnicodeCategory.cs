// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Globalization;

public static partial class CharExtension
{
    /// <summary>
    ///     Categorizes a specified Unicode character into a group identified by one of the  values.
    /// </summary>
    /// <param name="c">The Unicode character to categorize.</param>
    /// <returns>A  value that identifies the group that contains .</returns>
    public static UnicodeCategory GetUnicodeCategory(this Char c)
    {
        return Char.GetUnicodeCategory(c);
    }
}