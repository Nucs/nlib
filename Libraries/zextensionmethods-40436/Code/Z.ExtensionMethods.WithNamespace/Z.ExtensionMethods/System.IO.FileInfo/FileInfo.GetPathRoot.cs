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
        ///     Gets the root directory information of the specified @this.
        /// </summary>
        /// <param name="this">The @this from which to obtain root directory information.</param>
        /// <returns>
        ///     The root directory of <paramref name="this" />, such as "C:\", or null if <paramref name="this" /> is null,
        ///     or an empty string if
        ///     <paramref
        ///         name="this" />
        ///     does not contain root directory information.
        /// </returns>
        /// ###
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="this" /> contains one or more of the invalid
        ///     characters defined in
        ///     <see
        ///         cref="M:System.IO.Path.GetInvalidPathChars" />
        ///     .-or- <see cref="F:System.String.Empty" /> was passed to
        ///     <paramref
        ///         name="this" />
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
        ///               public class System_IO_FileInfo_GetPathRoot
        ///               {
        ///                   [TestMethod]
        ///                   public void GetPathRoot()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_GetDirectoryName&quot;, &quot;CreateDirectory.txt&quot;));
        ///           
        ///                       // Examples
        ///                       string result = @this.GetPathRoot();
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(Path.GetPathRoot(@this.FullName), result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static String GetPathRoot(this FileInfo @this)
        {
            return Path.GetPathRoot(@this.FullName);
        }
    }
}