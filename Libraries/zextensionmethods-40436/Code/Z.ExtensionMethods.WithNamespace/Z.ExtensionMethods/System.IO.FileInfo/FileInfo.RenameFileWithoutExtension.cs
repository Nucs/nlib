// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;

namespace Z.ExtensionMethods
{
    public static partial class FileInfoExtension
    {
        /// <summary>
        ///     A FileInfo extension method that rename file without extension.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="newName">Name of the new.</param>
        /// ###
        /// <returns>.</returns>
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
        ///               public class System_IO_FileInfo_RenameFileWithoutExtension
        ///               {
        ///                   [TestMethod]
        ///                   public void RenameFileWithoutExtension()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_RenameWithoutExtension.txt&quot;));
        ///                       var @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_RenameWithoutExtension2.txt&quot;));
        ///                       bool result1 = @thisNewFile.Exists;
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                       }
        ///           
        ///                       // Examples
        ///                       @this.RenameFileWithoutExtension(&quot;Examples_System_IO_FileInfo_RenameWithoutExtension2&quot;);
        ///           
        ///                       // Unit Test
        ///                       @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_RenameWithoutExtension2.txt&quot;));
        ///                       bool result2 = @thisNewFile.Exists;
        ///           
        ///                       Assert.IsFalse(result1);
        ///                       Assert.IsTrue(result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void RenameFileWithoutExtension(this FileInfo @this, string newName)
        {
            string fileName = string.Concat(newName, @this.Extension);
            string filePath = Path.Combine(@this.Directory.FullName, fileName);
            @this.MoveTo(filePath);
        }
    }
}