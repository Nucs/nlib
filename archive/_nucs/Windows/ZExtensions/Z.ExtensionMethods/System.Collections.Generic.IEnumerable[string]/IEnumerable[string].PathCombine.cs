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
    public static string PathCombine(this IEnumerable<string> @this) {
#if (NET35 || NET3 || NET2)
        var l = @this.ToList();
        var res = "";
        res = l.First();
        res = l.Skip(1).Aggregate(res, Path.Combine);
        return res;
#else

        return Path.Combine(@this.ToArray());
#endif
    }
}