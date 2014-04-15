// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class ArrayExtension
    {
        /// <summary>
        ///     A T[] extension method that clears at.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The arrayToClear to act on.</param>
        /// <param name="at">at.</param>
        public static void ClearAt<T>(this T[] @this, int at)
        {
            Array.Clear(@this, at, 1);
        }
    }
}