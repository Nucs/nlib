// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     A FileInfo extension method that gets directory name.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The directory name.</returns>
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
    ///               public class System_IO_FileInfo_GetDirectoryName
    ///               {
    ///                   [TestMethod]
    ///                   public void GetDirectoryName()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_GetDirectoryName&quot;, &quot;CreateDirectory.txt&quot;));
    ///           
    ///                       // Examples
    ///                       string result = @this.GetDirectoryName(); // return &quot;DirectoryInfo_GetDirectoryName&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(@this.Directory.Name, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static String GetDirectoryName(this FileInfo @this)
    {
        return @this.Directory.Name;
    }
}