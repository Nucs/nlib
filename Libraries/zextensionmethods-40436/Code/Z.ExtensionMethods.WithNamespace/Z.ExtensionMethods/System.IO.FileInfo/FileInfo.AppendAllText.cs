// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;
using System.Text;

namespace Z.ExtensionMethods
{
    public static partial class FileInfoExtension
    {
        /// <summary>
        ///     Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist,
        ///     this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="this">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// ###
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="this" /> is a zero-length string, contains only
        ///     white space, or contains one or more invalid characters as defined by
        ///     <see
        ///         cref="F:System.IO.Path.InvalidPathChars" />
        ///     .
        /// </exception>
        /// ###
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="this" /> is null.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified @this, file name, or both exceed the system-
        ///     defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file
        ///     names must be less than 260 characters.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The specified @this is invalid (for example, the
        ///     directory doesn?t exist or it is on an unmapped drive).
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// ###
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     <paramref name="this" /> specified a file that is
        ///     read-only.-or- This operation is not supported on the current platform.-or-
        ///     <paramref
        ///         name="this" />
        ///     specified a directory.-or- The caller does not have the required permission.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.FileNotFoundException">
        ///     The file specified in <paramref name="this" /> was not
        ///     found.
        /// </exception>
        /// ###
        /// <exception cref="T:System.NotSupportedException">
        ///     <paramref name="this" /> is in an invalid format.
        /// </exception>
        /// ###
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
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
        ///               public class System_IO_FileInfo_AppendAllText
        ///               {
        ///                   [TestMethod]
        ///                   public void AppendAllText()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_AppendAllText.txt&quot;));
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                       }
        ///           
        ///                       // Examples
        ///                       @this.AppendAllText(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, @this.ReadToEnd());
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void AppendAllText(this FileInfo @this, String contents)
        {
            File.AppendAllText(@this.FullName, contents);
        }

        /// <summary>
        ///     Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="this">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// ###
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="this" /> is a zero-length string, contains only
        ///     white space, or contains one or more invalid characters as defined by
        ///     <see
        ///         cref="F:System.IO.Path.InvalidPathChars" />
        ///     .
        /// </exception>
        /// ###
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="this" /> is null.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified @this, file name, or both exceed the system-
        ///     defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file
        ///     names must be less than 260 characters.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The specified @this is invalid (for example, the
        ///     directory doesn?t exist or it is on an unmapped drive).
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// ###
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     <paramref name="this" /> specified a file that is
        ///     read-only.-or- This operation is not supported on the current platform.-or-
        ///     <paramref
        ///         name="this" />
        ///     specified a directory.-or- The caller does not have the required permission.
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.FileNotFoundException">
        ///     The file specified in <paramref name="this" /> was not
        ///     found.
        /// </exception>
        /// ###
        /// <exception cref="T:System.NotSupportedException">
        ///     <paramref name="this" /> is in an invalid format.
        /// </exception>
        /// ###
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
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
        ///               public class System_IO_FileInfo_AppendAllText
        ///               {
        ///                   [TestMethod]
        ///                   public void AppendAllText()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_AppendAllText.txt&quot;));
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                       }
        ///           
        ///                       // Examples
        ///                       @this.AppendAllText(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, @this.ReadToEnd());
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void AppendAllText(this FileInfo @this, String contents, Encoding encoding)
        {
            File.AppendAllText(@this.FullName, contents, encoding);
        }
    }
}