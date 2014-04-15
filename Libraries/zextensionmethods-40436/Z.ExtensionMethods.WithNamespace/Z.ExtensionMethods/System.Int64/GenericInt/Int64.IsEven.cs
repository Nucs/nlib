// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    /// ###
    /// <summary>Int 64 extension.</summary>
    public static partial class Int64Extension
    {
        /// <summary>
        ///     An Int64 extension method that query if '@this' is even.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if even, false if not.</returns>
        public static bool IsEven(this Int64 @this)
        {
            return @this%2 == 0;
        }
    }
}