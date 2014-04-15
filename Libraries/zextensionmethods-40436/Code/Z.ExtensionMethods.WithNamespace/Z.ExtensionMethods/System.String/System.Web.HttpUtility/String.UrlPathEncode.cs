// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Web;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     Encodes the path portion of a URL string for reliable HTTP transmission from the Web server to a client.
        /// </summary>
        /// <param name="str">The text to encode.</param>
        /// <returns>The encoded text.</returns>
        public static String UrlPathEncode(this String str)
        {
            return HttpUtility.UrlPathEncode(str);
        }
    }
}