// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that converts the @this to a unique identifier.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a GUID.</returns>
        public static Guid ToGuid(this object @this)
        {
            return new Guid(@this.ToString());
        }
    }
}