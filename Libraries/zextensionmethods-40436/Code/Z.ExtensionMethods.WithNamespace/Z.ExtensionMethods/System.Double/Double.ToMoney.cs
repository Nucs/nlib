// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     A Double extension method that converts the @this to a money.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Double.</returns>
        public static Double ToMoney(this Double @this)
        {
            return Math.Round(@this, 2);
        }
    }
}