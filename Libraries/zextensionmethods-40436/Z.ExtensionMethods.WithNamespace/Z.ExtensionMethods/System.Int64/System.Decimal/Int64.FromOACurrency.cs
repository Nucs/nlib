// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods
{
    public static partial class Int64Extension
    {
        /// <summary>
        ///     Converts the specified 64-bit signed integer, which contains an OLE Automation Currency value, to the
        ///     equivalent  value.
        /// </summary>
        /// <param name="cy">An OLE Automation Currency value.</param>
        /// <returns>A  that contains the equivalent of .</returns>
        public static Decimal FromOACurrency(this Int64 cy)
        {
            return Decimal.FromOACurrency(cy);
        }
    }
}