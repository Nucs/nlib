// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    /// ###
    /// <summary>Int 16 extension.</summary>
    public static partial class Int16Extension
    {
        /// <summary>
        ///     An Int16 extension method that query if '@this' is multiple of.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>true if multiple of, false if not.</returns>
        public static bool IsMultipleOf(this Int16 @this, Int16 factor)
        {
            return @this%factor == 0;
        }
    }
}