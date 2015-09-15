// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

public static partial class ICollectionExtension
{
    /// <summary>
    ///     An ICollection&lt;T&gt; extension method that adds a range of values that's not already in the ICollection.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="values">A variable-length parameters list containing values.</param>
    public static void AddRangeIfNotContains<T>(this ICollection<T> @this, params T[] values)
    {
        foreach (T value in values)
        {
            if (!@this.Contains(value))
            {
                @this.Add(value);
            }
        }
    }
}