// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class TimeSpanExtension
    {
        /// <summary>
        ///     A TimeSpan extension method that add the specified TimeSpan to the current UTC (Coordinated Universal Time)
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan added to it.</returns>
        public static DateTime UtcFromNow(this TimeSpan @this)
        {
            return DateTime.UtcNow.Add(@this);
        }
    }
}