// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;
using System.Text;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     Creates a new file, writes the specified string to the file, and then closes the file. If the target file
    ///     already exists, it is overwritten.
    /// </summary>
    /// <param name="this">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
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
    ///     <paramref name="this" /> is null or
    ///     <paramref name="contents" /> is empty.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.PathTooLongException">
    ///     The specified @this, file name, or both exceed the system-
    ///     defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file
    ///     names must be less than 260 characters.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    ///     The specified @this is invalid (for example, it is on
    ///     an unmapped drive).
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
    ///               public class System_IO_FileInfo_WriteAllText
    ///               {
    ///                   [TestMethod]
    ///                   public void WriteAllText()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_WriteAllText.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.WriteAllText(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void WriteAllText(this FileInfo @this, String contents)
    {
        File.WriteAllText(@this.FullName, contents);
    }

    /// <summary>
    ///     Creates a new file, writes the specified string to the file using the specified encoding, and then closes the
    ///     file. If the target file already exists, it is overwritten.
    /// </summary>
    /// <param name="this">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="encoding">The encoding to apply to the string.</param>
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
    ///     <paramref name="this" /> is null or
    ///     <paramref name="contents" /> is empty.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.PathTooLongException">
    ///     The specified @this, file name, or both exceed the system-
    ///     defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file
    ///     names must be less than 260 characters.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    ///     The specified @this is invalid (for example, it is on
    ///     an unmapped drive).
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
    ///               public class System_IO_FileInfo_WriteAllText
    ///               {
    ///                   [TestMethod]
    ///                   public void WriteAllText()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_WriteAllText.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.WriteAllText(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void WriteAllText(this FileInfo @this, String contents, Encoding encoding)
    {
        File.WriteAllText(@this.FullName, contents, encoding);
    }
}