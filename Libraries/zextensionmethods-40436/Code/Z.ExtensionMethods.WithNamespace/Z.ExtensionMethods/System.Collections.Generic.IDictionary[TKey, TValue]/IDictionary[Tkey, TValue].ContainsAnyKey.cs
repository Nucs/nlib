// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.ExtensionMethods
{
    public static partial class IDictionaryExtension
    {
        /// <summary>
        ///     An IDictionary&lt;TKey,TValue&gt; extension method that query if '@this' contains any key.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="keys">A variable-length parameters list containing keys.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAnyKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, params TKey[] keys)
        {
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (TKey value in keys)
// ReSharper restore LoopCanBeConvertedToQuery
            {
                if (@this.ContainsKey(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}