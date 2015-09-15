// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     Creates a new file, write the specified string array to the file, and then closes the file.
    /// </summary>
    /// <param name="this">The file to write to.</param>
    /// <param name="contents">The string array to write to the file.</param>
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
    ///     Either <paramref name="this" /> or
    ///     <paramref name="contents" /> is null.
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
    ///               public class System_IO_FileInfo_WriteAllLines
    ///               {
    ///                   [TestMethod]
    ///                   public void WriteAllLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_WriteAllLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.WriteAllLines(new[] {&quot;Fizz&quot;, &quot;Buzz&quot;});
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot; + Environment.NewLine, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void WriteAllLines(this FileInfo @this, String[] contents)
    {
        File.WriteAllLines(@this.FullName, contents);
    }

    /// <summary>
    ///     Creates a new file, writes the specified string array to the file by using the specified encoding, and then
    ///     closes the file.
    /// </summary>
    /// <param name="this">The file to write to.</param>
    /// <param name="contents">The string array to write to the file.</param>
    /// <param name="encoding">
    ///     An <see cref="T:System.Text.Encoding" /> object that represents the character encoding
    ///     applied to the string array.
    /// </param>
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
    ///     Either <paramref name="this" /> or
    ///     <paramref name="contents" /> is null.
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
    ///               public class System_IO_FileInfo_WriteAllLines
    ///               {
    ///                   [TestMethod]
    ///                   public void WriteAllLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_WriteAllLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.WriteAllLines(new[] {&quot;Fizz&quot;, &quot;Buzz&quot;});
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot; + Environment.NewLine, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void WriteAllLines(this FileInfo @this, String[] contents, Encoding encoding)
    {
        File.WriteAllLines(@this.FullName, contents, encoding);
    }

    /// <summary>
    ///     Creates a new file, write the specified string array to the file, and then closes the file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="contents">The string array to write to the file.</param>
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
    ///               public class System_IO_FileInfo_WriteAllLines
    ///               {
    ///                   [TestMethod]
    ///                   public void WriteAllLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_WriteAllLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.WriteAllLines(new[] {&quot;Fizz&quot;, &quot;Buzz&quot;});
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot; + Environment.NewLine, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void WriteAllLines(this FileInfo @this, IEnumerable<String> contents)
    {
        File.WriteAllLines(@this.FullName, contents.ToArray());
    }

    /// <summary>
    ///     Creates a new file, writes the specified string array to the file by using the specified encoding, and then
    ///     closes the file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="contents">The string array to write to the file.</param>
    /// <param name="encoding">
    ///     An <see cref="T:System.Text.Encoding" /> object that represents the character encoding
    ///     applied to the string array.
    /// </param>
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
    ///               public class System_IO_FileInfo_WriteAllLines
    ///               {
    ///                   [TestMethod]
    ///                   public void WriteAllLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_WriteAllLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.WriteAllLines(new[] {&quot;Fizz&quot;, &quot;Buzz&quot;});
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot; + Environment.NewLine, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void WriteAllLines(this FileInfo @this, IEnumerable<String> contents, Encoding encoding)
    {
        File.WriteAllLines(@this.FullName, contents.ToArray(), encoding);
    }
}