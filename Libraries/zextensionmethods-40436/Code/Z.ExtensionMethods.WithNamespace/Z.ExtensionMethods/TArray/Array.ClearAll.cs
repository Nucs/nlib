// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class ArrayExtension
    {
        /// <summary>
        ///     A T[] extension method that clears all described by @this.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// ###
        /// <returns>.</returns>
        public static void ClearAll<T>(this T[] @this)
        {
            Array.Clear(@this, 0, @this.Length);
        }
    }
}