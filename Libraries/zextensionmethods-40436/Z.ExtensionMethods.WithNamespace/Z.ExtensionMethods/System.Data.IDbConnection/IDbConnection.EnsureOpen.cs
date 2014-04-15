// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

namespace Z.ExtensionMethods
{
    public static class IDbConnectionExtension
    {
        /// <summary>
        ///     An IDbConnection extension method that ensures that open.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void EnsureOpen(this IDbConnection @this)
        {
            if (@this.State == ConnectionState.Closed)
            {
                @this.Open();
            }
        }
    }
}