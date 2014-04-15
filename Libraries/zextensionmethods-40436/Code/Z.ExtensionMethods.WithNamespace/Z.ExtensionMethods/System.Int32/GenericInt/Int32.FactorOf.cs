// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    /// ###
    /// <summary>Int 32 extension.</summary>
    public static partial class Int32Extension
    {
        /// <summary>
        ///     An Int32 extension method that factor of.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="factorNumer">The factor numer.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool FactorOf(this Int32 @this, Int32 factorNumer)
        {
            return factorNumer%@this == 0;
        }
    }
}