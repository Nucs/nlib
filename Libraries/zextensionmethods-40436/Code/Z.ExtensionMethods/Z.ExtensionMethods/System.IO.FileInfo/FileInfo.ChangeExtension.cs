// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     Changes the extension of a @this string.
    /// </summary>
    /// <param name="this">
    ///     The @this information to modify. The @this cannot contain any of the characters defined in
    ///     <see
    ///         cref="M:System.IO.Path.GetInvalidPathChars" />
    ///     .
    /// </param>
    /// <param name="extension">
    ///     The new extension (with or without a leading period). Specify null to remove an existing
    ///     extension from
    ///     <paramref
    ///         name="this" />
    ///     .
    /// </param>
    /// <returns>
    ///     The modified @this information.On Windows-based desktop platforms, if <paramref name="this" /> is null or an
    ///     empty string (""), the @this information is returned unmodified. If
    ///     <paramref
    ///         name="extension" />
    ///     is null, the returned string contains the specified @this with its extension removed. If
    ///     <paramref
    ///         name="this" />
    ///     has no extension, and <paramref name="extension" /> is not null, the returned @this string contains
    ///     <paramref
    ///         name="extension" />
    ///     appended to the end of <paramref name="this" />.
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
    ///               public class System_IO_FileInfo_ChangeExtension
    ///               {
    ///                   [TestMethod]
    ///                   public void ChangeExtension()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ChangeExtension.txt&quot;));
    ///                       var @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ChangeExtension.cs&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       string result = @this.ChangeExtension(&quot;cs&quot;);
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(@thisNewFile.FullName, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static String ChangeExtension(this FileInfo @this, String extension)
    {
        return Path.ChangeExtension(@this.FullName, extension);
    }
}