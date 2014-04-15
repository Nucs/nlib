// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Text;
using System.Web;

public static partial class StringExtension
{
    /// <summary>
    ///     Converts a URL-encoded string into a decoded array of bytes.
    /// </summary>
    /// <param name="str">The string to decode.</param>
    /// <returns>A decoded array of bytes.</returns>
    public static Byte[] UrlDecodeToBytes(this String str)
    {
        return HttpUtility.UrlDecodeToBytes(str);
    }

    /// <summary>
    ///     Converts a URL-encoded string into a decoded array of bytes using the specified decoding object.
    /// </summary>
    /// <param name="str">The string to decode.</param>
    /// <param name="e">The  object that specifies the decoding scheme.</param>
    /// <returns>A decoded array of bytes.</returns>
    public static Byte[] UrlDecodeToBytes(this String str, Encoding e)
    {
        return HttpUtility.UrlDecodeToBytes(str, e);
    }
}