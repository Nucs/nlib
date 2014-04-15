// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns the base 10 logarithm of a specified number.
        /// </summary>
        /// <param name="d">A number whose logarithm is to be found.</param>
        /// <returns>
        ///     One of the values in the following table.  parameter Return value Positive The base 10 log of ; that is, log
        ///     10. Zero Negative Equal to Equal to.
        /// </returns>
        public static Double Log10(this Double d)
        {
            return Math.Log10(d);
        }
    }
}