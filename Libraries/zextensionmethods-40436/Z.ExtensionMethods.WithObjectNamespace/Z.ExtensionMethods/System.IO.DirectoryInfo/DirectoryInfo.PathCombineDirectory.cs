// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.IO;
using System.Linq;

public static partial class DirectoryInfoExtension
{
    /// <summary>
    ///     Combines multiples string into a path.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="paths">A variable-length parameters list containing paths.</param>
    /// <returns>
    ///     The combined paths as a DirectoryInfo. If one of the specified paths is a zero-length string, this method
    ///     returns the other path.
    /// </returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.IO;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_DirectoryInfo_PathCombineDirectory
    ///               {
    ///                   [TestMethod]
    ///                   public void PathCombine()
    ///                   {
    ///                       // Type
    ///                       var @this = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
    ///           
    ///                       string path1 = &quot;Fizz&quot;;
    ///                       string path2 = &quot;Buzz&quot;;
    ///           
    ///                       // Exemples
    ///                       DirectoryInfo result = @this.PathCombineDirectory(path1, path2); // Combine path1 and path2 with the DirectoryInfo
    ///           
    ///                       // Unit Test
    ///                       var expected = new DirectoryInfo(Path.Combine(@this.FullName, path1, path2));
    ///                       Assert.AreEqual(expected.FullName, result.FullName);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static DirectoryInfo PathCombineDirectory(this DirectoryInfo @this, params string[] paths)
    {
        List<string> list = paths.ToList();
        list.Insert(0, @this.FullName);
        return new DirectoryInfo(Path.Combine(list.ToArray()));
    }
}