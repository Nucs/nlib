// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;
using System.Linq;

namespace Z.ExtensionMethods
{
    public static partial class DirectoryInfoExtension
    {
        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">The search string to match against the names of directories in.</param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref
        ///         name="this" />
        ///     and that match the specified search pattern.
        /// </returns>
        /// ###
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// ###
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="this " />is a zero-length string, contains only
        ///     white space, or contains invalid characters as defined by
        ///     <see
        ///         cref="M:System.IO.Path.GetInvalidPathChars" />
        ///     .- or -<paramref name="searchPattern" /> does not contain a valid pattern.
        /// </exception>
        /// ###
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="this" /> is null.-or-
        ///     <paramref name="searchPattern" /> is null.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     <paramref name="this" /> is invalid, such as
        ///     referring to an unmapped drive.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.IOException">
        ///     <paramref name="this" /> is a file name.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified @this, file name, or combined exceed the
        ///     system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters
        ///     and file names must be less than 260 characters.
        /// </exception>
        /// ###
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// ###
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
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
        ///               public class System_IO_DirectoryInfo_GetDirectories
        ///               {
        ///                   [TestMethod]
        ///                   public void GetDirectories()
        ///                   {
        ///                       // Type
        ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_GetDirectories&quot;));
        ///                       Directory.CreateDirectory(root.FullName);
        ///                       root.CreateSubdirectory(&quot;DirFizz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirBuzz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirNotFound123&quot;);
        ///           
        ///                       // Exemples
        ///                       DirectoryInfo[] result = root.GetDirectories(new[] {&quot;DirFizz*&quot;, &quot;DirBuzz*&quot;});
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(2, result.Length);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static DirectoryInfo[] GetDirectories(this DirectoryInfo @this, String[] searchPatterns)
        {
            return searchPatterns.SelectMany(x => @this.GetDirectories(x)).Distinct().ToArray();
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search pattern in a specified @this, and
        ///     optionally searches subdirectories.
        /// </summary>
        /// <param name="this">The directory to search.</param>
        /// <param name="searchPatterns">
        ///     The search string to match against the names of directories in
        ///     <paramref name="this" />.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation should
        ///     include only the current directory or should include all subdirectories.The default value is
        ///     <see
        ///         cref="F:System.IO.SearchOption.TopDirectoryOnly" />
        ///     .
        /// </param>
        /// <returns>
        ///     An enumerable collection of the full names (including paths) for the directories in the directory specified by
        ///     <paramref
        ///         name="this" />
        ///     and that match the specified search pattern and option.
        /// </returns>
        /// ###
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="this " />is a zero-length string, contains only
        ///     white space, or contains invalid characters as defined by
        ///     <see
        ///         cref="M:System.IO.Path.GetInvalidPathChars" />
        ///     .- or -<paramref name="searchPattern" /> does not contain a valid pattern.
        /// </exception>
        /// ###
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="this" /> is null.-or-
        ///     <paramref name="searchPattern" /> is null.
        /// </exception>
        /// ###
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="searchOption" /> is not a valid
        ///     <see cref="T:System.IO.SearchOption" /> value.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     <paramref name="this" /> is invalid, such as
        ///     referring to an unmapped drive.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.IOException">
        ///     <paramref name="this" /> is a file name.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified @this, file name, or combined exceed the
        ///     system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters
        ///     and file names must be less than 260 characters.
        /// </exception>
        /// ###
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// ###
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
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
        ///               public class System_IO_DirectoryInfo_GetDirectories
        ///               {
        ///                   [TestMethod]
        ///                   public void GetDirectories()
        ///                   {
        ///                       // Type
        ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_GetDirectories&quot;));
        ///                       Directory.CreateDirectory(root.FullName);
        ///                       root.CreateSubdirectory(&quot;DirFizz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirBuzz123&quot;);
        ///                       root.CreateSubdirectory(&quot;DirNotFound123&quot;);
        ///           
        ///                       // Exemples
        ///                       DirectoryInfo[] result = root.GetDirectories(new[] {&quot;DirFizz*&quot;, &quot;DirBuzz*&quot;});
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(2, result.Length);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static DirectoryInfo[] GetDirectories(this DirectoryInfo @this, String[] searchPatterns, SearchOption searchOption)
        {
            return searchPatterns.SelectMany(x => @this.GetDirectories(x, searchOption)).Distinct().ToArray();
        }
    }
}