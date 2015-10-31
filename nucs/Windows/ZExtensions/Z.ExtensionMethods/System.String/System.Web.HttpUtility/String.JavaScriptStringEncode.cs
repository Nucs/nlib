// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).
#if !(NET_3_5 || NET_3_0 || NET_2_0)

using System;
using System.Web;

public static partial class StringExtension
{
    /// <summary>
    ///     Encodes a string.
    /// </summary>
    /// <param name="value">A string to encode.</param>
    /// <returns>An encoded string.</returns>
    public static String JavaScriptStringEncode(this String value)
    {
        return HttpUtility.JavaScriptStringEncode(value);
    }

    /// <summary>
    ///     Encodes a string.
    /// </summary>
    /// <param name="value">A string to encode.</param>
    /// <param name="addDoubleQuotes">
    ///     A value that indicates whether double quotation marks will be included around the
    ///     encoded string.
    /// </param>
    /// <returns>An encoded string.</returns>
    public static String JavaScriptStringEncode(this String value, Boolean addDoubleQuotes)
    {
        return HttpUtility.JavaScriptStringEncode(value, addDoubleQuotes);
    }
}
#endif