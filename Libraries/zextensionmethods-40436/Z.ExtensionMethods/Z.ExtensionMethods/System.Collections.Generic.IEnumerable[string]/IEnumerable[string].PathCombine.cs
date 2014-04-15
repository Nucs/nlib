// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.IO;
using System.Linq;

public static partial class IEnumerableExtension
{
    /// <summary>
    ///     An IEnumerable&lt;string&gt; extension method that combine all value to return a path.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The path.</returns>
    public static string PathCombine(this IEnumerable<string> @this)
    {
        return Path.Combine(@this.ToArray());
    }
}