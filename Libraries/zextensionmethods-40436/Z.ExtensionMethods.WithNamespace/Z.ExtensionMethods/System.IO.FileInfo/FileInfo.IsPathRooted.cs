// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;

namespace Z.ExtensionMethods
{
    public static partial class FileInfoExtension
    {
        /// <summary>
        ///     Gets a value indicating whether the specified @this string contains a root.
        /// </summary>
        /// <param name="this">The @this to test.</param>
        /// <returns>
        ///     true if <paramref name="this" /> contains a root; otherwise, false.
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
        ///               public class System_IO_FileInfo_IsPathRooted
        ///               {
        ///                   [TestMethod]
        ///                   public void IsPathRooted()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_GetDirectoryName&quot;, &quot;CreateDirectory.txt&quot;));
        ///           
        ///                       // Examples
        ///                       bool result = @this.IsPathRooted();
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(Path.IsPathRooted(@this.FullName), result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static Boolean IsPathRooted(this FileInfo @this)
        {
            return Path.IsPathRooted(@this.FullName);
        }
    }
}