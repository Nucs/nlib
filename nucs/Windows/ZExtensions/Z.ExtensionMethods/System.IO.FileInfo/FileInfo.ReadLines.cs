// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     Reads the lines of a file.
    /// </summary>
    /// <param name="this">The file to read.</param>
    /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
    /// ###
    /// <exception cref="T:System.ArgumentException">
    ///     <paramref name="this" /> is a zero-length string, contains only
    ///     white space, or contains one or more invalid characters defined by the
    ///     <see
    ///         cref="M:System.IO.Path.GetInvalidPathChars" />
    ///     method.
    /// </exception>
    /// ###
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="this" /> is null.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    ///     <paramref name="this" /> is invalid (for example, it
    ///     is on an unmapped drive).
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.FileNotFoundException">
    ///     The file specified by <paramref name="this" /> was not
    ///     found.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// ###
    /// <exception cref="T:System.IO.PathTooLongException">
    ///     <paramref name="this" /> exceeds the system-defined
    ///     maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names
    ///     must be less than 260 characters.
    /// </exception>
    /// ###
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// ###
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///     <paramref name="this" /> specifies a file that is
    ///     read-only.-or-This operation is not supported on the current platform.-or-
    ///     <paramref
    ///         name="this" />
    ///     is a directory.-or-The caller does not have the required permission.
    /// </exception>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.Collections.Generic;
    ///           using System.IO;
    ///           using System.Linq;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_FileInfo_ReadLines
    ///               {
    ///                   [TestMethod]
    ///                   public void ReadLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                           byte[] byteToWrites = Encoding.Default.GetBytes(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///                           stream.Write(byteToWrites, 0, byteToWrites.Length);
    ///                       }
    ///           
    ///                       // Examples
    ///                       List&lt;string&gt; result = @this.ReadLines().ToList(); // return new [] {&quot;Fizz&quot;, &quot;Buzz&quot;};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result[0]);
    ///                       Assert.AreEqual(&quot;Buzz&quot;, result[1]);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static IEnumerable<String> ReadLines(this FileInfo @this)
    {
#if (NET_3_5 || NET_3_0 || NET_2_0)
        return File.ReadAllLines(@this.FullName);
#else
        return File.ReadLines(@this.FullName);

#endif
    }

    /// <summary>
    ///     Read the lines of a file that has a specified encoding.
    /// </summary>
    /// <param name="this">The file to read.</param>
    /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
    /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
    /// ###
    /// <exception cref="T:System.ArgumentException">
    ///     <paramref name="this" /> is a zero-length string, contains only
    ///     white space, or contains one or more invalid characters as defined by the
    ///     <see
    ///         cref="M:System.IO.Path.GetInvalidPathChars" />
    ///     method.
    /// </exception>
    /// ###
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="this" /> is null.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    ///     <paramref name="this" /> is invalid (for example, it
    ///     is on an unmapped drive).
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.FileNotFoundException">
    ///     The file specified by <paramref name="this" /> was not
    ///     found.
    /// </exception>
    /// ###
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// ###
    /// <exception cref="T:System.IO.PathTooLongException">
    ///     <paramref name="this" /> exceeds the system-defined
    ///     maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names
    ///     must be less than 260 characters.
    /// </exception>
    /// ###
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// ###
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///     <paramref name="this" /> specifies a file that is
    ///     read-only.-or-This operation is not supported on the current platform.-or-
    ///     <paramref
    ///         name="this" />
    ///     is a directory.-or-The caller does not have the required permission.
    /// </exception>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.Collections.Generic;
    ///           using System.IO;
    ///           using System.Linq;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_FileInfo_ReadLines
    ///               {
    ///                   [TestMethod]
    ///                   public void ReadLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                           byte[] byteToWrites = Encoding.Default.GetBytes(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///                           stream.Write(byteToWrites, 0, byteToWrites.Length);
    ///                       }
    ///           
    ///                       // Examples
    ///                       List&lt;string&gt; result = @this.ReadLines().ToList(); // return new [] {&quot;Fizz&quot;, &quot;Buzz&quot;};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result[0]);
    ///                       Assert.AreEqual(&quot;Buzz&quot;, result[1]);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static IEnumerable<String> ReadLines(this FileInfo @this, Encoding encoding)
    {
#if (NET_3_5 || NET_3_0 || NET_2_0)
        return File.ReadAllLines(@this.FullName, encoding);
#else
        return File.ReadLines(@this.FullName, encoding);
#endif
    }
}