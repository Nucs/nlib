// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ArrayExtension
{
    /// <summary>
    ///     Returns the number of bytes in the specified array.
    /// </summary>
    /// <param name="array">An array.</param>
    /// <returns>The number of bytes in the array.</returns>
    public static Int32 ByteLength(this Array array)
    {
        return Buffer.ByteLength(array);
    }
}