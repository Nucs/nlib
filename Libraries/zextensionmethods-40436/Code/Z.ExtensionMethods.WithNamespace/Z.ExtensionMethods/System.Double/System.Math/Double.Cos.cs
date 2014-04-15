// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="d">An angle, measured in radians.</param>
        /// <returns>The cosine of . If  is equal to , , or , this method returns .</returns>
        public static Double Cos(this Double d)
        {
            return Math.Cos(d);
        }
    }
}