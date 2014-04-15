// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that query if '@this' is valid sbyte.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if valid sbyte, false if not.</returns>
        public static bool IsValidSByte(this object @this)
        {
            if (@this == null)
            {
                return true;
            }

            sbyte result;
            return sbyte.TryParse(@this.ToString(), out result);
        }
    }
}