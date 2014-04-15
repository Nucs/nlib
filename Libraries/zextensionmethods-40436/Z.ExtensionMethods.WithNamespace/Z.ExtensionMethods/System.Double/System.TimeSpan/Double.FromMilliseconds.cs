// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns a  that represents a specified number of milliseconds.
        /// </summary>
        /// <param name="value">A number of milliseconds.</param>
        /// <returns>An object that represents .</returns>
        public static TimeSpan FromMilliseconds(this Double value)
        {
            return TimeSpan.FromMilliseconds(value);
        }
    }
}