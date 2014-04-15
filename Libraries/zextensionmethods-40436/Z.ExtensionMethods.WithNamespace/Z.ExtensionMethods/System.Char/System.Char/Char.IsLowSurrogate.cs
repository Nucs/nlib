// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class CharExtension
    {
        /// <summary>
        ///     Indicates whether the specified  object is a low surrogate.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns>
        ///     true if the numeric value of the  parameter ranges from U+DC00 through U+DFFF; otherwise, false.
        /// </returns>
        public static Boolean IsLowSurrogate(this Char c)
        {
            return Char.IsLowSurrogate(c);
        }
    }
}