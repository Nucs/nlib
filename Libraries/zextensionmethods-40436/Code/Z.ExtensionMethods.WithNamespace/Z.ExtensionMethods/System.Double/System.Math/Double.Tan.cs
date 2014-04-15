// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class DoubleExtension
    {
        /// <summary>
        ///     Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="a">An angle, measured in radians.</param>
        /// <returns>The tangent of . If  is equal to , , or , this method returns .</returns>
        public static Double Tan(this Double a)
        {
            return Math.Tan(a);
        }
    }
}