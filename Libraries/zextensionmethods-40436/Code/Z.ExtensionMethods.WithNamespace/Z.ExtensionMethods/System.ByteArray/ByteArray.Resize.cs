// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    /// ###
    /// <summary>Byte array extension.</summary>
    public static partial class ByteArrayExtension
    {
        /// <summary>
        ///     A byte[] extension method that resizes the byte[].
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="newSize">Size of the new.</param>
        /// <returns>A byte[].</returns>
        public static byte[] Resize(this byte[] @this, int newSize)
        {
            Array.Resize(ref @this, newSize);
            return @this;
        }
    }
}