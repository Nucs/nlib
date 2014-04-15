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
        ///     Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="this">The file to open for reading.</param>
        /// <returns>A byte array containing the contents of the file.</returns>
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
        ///     The specified @this is invalid (for example, it is on
        ///     an unmapped drive).
        /// </exception>
        /// ###
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// ###
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     This operation is not supported on the current
        ///     platform.-or- <paramref name="this" /> specified a directory.-or- The caller does not have the required
        ///     permission.
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
        ///               public class System_IO_FileInfo_ReadAllBytes
        ///               {
        ///                   [TestMethod]
        ///                   public void ReadAllBytes()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadAllBytes.txt&quot;));
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                           stream.WriteByte(0);
        ///                       }
        ///           
        ///                       // Examples
        ///                       byte[] value = @this.ReadAllBytes(); // return byte[] { 0 };
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(0, value[0]);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static Byte[] ReadAllBytes(this FileInfo @this)
        {
            return File.ReadAllBytes(@this.FullName);
        }
    }
}