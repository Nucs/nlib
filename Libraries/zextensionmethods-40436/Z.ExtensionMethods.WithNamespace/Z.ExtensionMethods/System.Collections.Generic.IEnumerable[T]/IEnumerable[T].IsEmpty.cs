// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Linq;

namespace Z.ExtensionMethods
{
    public static partial class IEnumerableExtension
    {
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that query if 'collection' is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="collection">The collection to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }
    }
}