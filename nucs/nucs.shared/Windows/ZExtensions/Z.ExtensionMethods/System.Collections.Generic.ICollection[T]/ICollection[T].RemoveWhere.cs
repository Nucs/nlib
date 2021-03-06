// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Linq;

public static partial class ICollectionExtension {
    /// <summary>
    ///     An ICollection&lt;T&gt; extension method that removes value that satisfy the predicate.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <returns>The amount of items found and removed</returns>
    /// <param name="this">The @this to act on.</param>
    /// <param name="predicate">The predicate.</param>
    public static int RemoveWhere<T>(this ICollection<T> @this, Func<T, bool> predicate) {
        
        List<T> list = @this.Where(predicate).ToList();
        int c = list.Count;
        foreach (T item in list) 
            @this.Remove(item);
        return c;
    }
}