// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.IO;

namespace Z.ExtensionMethods
{
    public static partial class DirectoryInfoEnumerableExtension
    {
        /// <summary>
        ///     An IEnumerable&lt;DirectoryInfo&gt; extension method that deletes the given @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void Delete(this IEnumerable<DirectoryInfo> @this)
        {
            foreach (DirectoryInfo t in @this)
            {
                t.Delete();
            }
        }
    }
}