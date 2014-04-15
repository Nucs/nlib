// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class SingleExtension
    {
        /// <summary>
        ///     Returns a value indicating whether the specified number evaluates to negative or positive infinity.
        /// </summary>
        /// <param name="f">A single-precision floating-point number.</param>
        /// <returns>true if  evaluates to  or ; otherwise, false.</returns>
        public static Boolean IsInfinity(this Single f)
        {
            return Single.IsInfinity(f);
        }
    }
}