// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

namespace Z.ExtensionMethods
{
    public static partial class ConnectionStateExtension
    {
        /// <summary>
        ///     A ConnectionState extension method that not in.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool NotIn(this ConnectionState @this, params ConnectionState[] values)
        {
            return values.IndexOf(@this) == -1;
        }
    }
}