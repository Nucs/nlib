// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     Indicates whether the character at the specified position in a specified string is categorized as a
        ///     punctuation mark.
        /// </summary>
        /// <param name="s">A string.</param>
        /// <param name="index">The position of the character to evaluate in .</param>
        /// <returns>true if the character at position  in  is a punctuation mark; otherwise, false.</returns>
        public static Boolean IsPunctuation(this String s, Int32 index)
        {
            return Char.IsPunctuation(s, index);
        }
    }
}