// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     Determines whether a @this includes a file name extension.
    /// </summary>
    /// <param name="this">The @this to search for an extension.</param>
    /// <returns>
    ///     true if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the
    ///     @this include a period (.) followed by one or more characters; otherwise, false.
    /// </returns>
    /// ###
    /// <exception cref="T:System.ArgumentException">
    ///     <paramref name="this" /> contains one or more of the invalid
    ///     characters defined in
    ///     <see
    ///         cref="M:System.IO.Path.GetInvalidPathChars" />
    ///     .
    /// </exception>
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
    ///               public class System_IO_FileInfo_HasExtension
    ///               {
    ///                   [TestMethod]
    ///                   public void HasExtension()
    ///                   {
    ///                       // Type
    ///                       var file1 = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_GetDirectoryName&quot;, &quot;CreateDirectory.txt&quot;));
    ///                       var file2 = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_GetDirectoryName&quot;, &quot;CreateDirectory&quot;));
    ///           
    ///                       // Examples
    ///                       bool result1 = file1.HasExtension(); // return true;
    ///                       bool result2 = file2.HasExtension(); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(result1);
    ///                       Assert.IsFalse(result2);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static Boolean HasExtension(this FileInfo @this)
    {
        return Path.HasExtension(@this.FullName);
    }
}