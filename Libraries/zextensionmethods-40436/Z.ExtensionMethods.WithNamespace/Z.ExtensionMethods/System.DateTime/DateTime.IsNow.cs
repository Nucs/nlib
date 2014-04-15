// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DateTimeExtension
    {
        /// <summary>
        ///     A DateTime extension method that query if '@this' is now.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if now, false if not.</returns>
        public static bool IsNow(this DateTime @this)
        {
            return @this == DateTime.Now;
        }
    }
}