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
    ///               public class System_IO_FileInfo_RenameExtension
    ///               {
    ///                   [TestMethod]
    ///                   public void Rename()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_RenameExtension.txt&quot;));
    ///                       var @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_RenameExtension.cs&quot;));
    ///                       bool result1 = @thisNewFile.Exists;
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.RenameExtension(&quot;cs&quot;);
    ///           
    ///                       // Unit Test
    ///                       @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_RenameExtension.cs&quot;));
    ///                       bool result2 = @thisNewFile.Exists;
    ///           
    ///                       Assert.IsFalse(result1);
    ///                       Assert.IsTrue(result2);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void RenameExtension(this FileInfo @this, String extension)
    {
        string filePath = Path.ChangeExtension(@this.FullName, extension);
        @this.MoveTo(filePath);
    }
}