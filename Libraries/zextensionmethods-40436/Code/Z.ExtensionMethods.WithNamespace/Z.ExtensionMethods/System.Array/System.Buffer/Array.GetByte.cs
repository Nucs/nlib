// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class ArrayExtension
    {
        /// <summary>
        ///     Retrieves the byte at a specified location in a specified array.
        /// </summary>
        /// <param name="array">An array.</param>
        /// <param name="index">A location in the array.</param>
        /// <returns>Returns the  byte in the array.</returns>
        public static Byte GetByte(this Array array, Int32 index)
        {
            return Buffer.GetByte(array, index);
        }
    }
}