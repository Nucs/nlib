// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Linq;

public static partial class DirectoryInfoExtension
{
    /// <summary>
    ///     A DirectoryInfo extension method that gets a size.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The size.</returns>
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
    ///               public class System_IO_DirectoryInfo_GetSize
    ///               {
    ///                   [TestMethod]
    ///                   public void GetSize()
    ///                   {
    ///                       // Type
    ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_GetFiles&quot;));
    ///                       Directory.CreateDirectory(root.FullName);
    ///           
    ///                       var file1 = new FileInfo(Path.Combine(root.FullName, &quot;test.txt&quot;));
    ///                       var file2 = new FileInfo(Path.Combine(root.FullName, &quot;test.cs&quot;));
    ///                       var file3 = new FileInfo(Path.Combine(root.FullName, &quot;test.asp&quot;));
    ///                       file1.Create();
    ///                       file2.Create();
    ///                       file3.Create();
    ///           
    ///                       // Exemples
    ///                       long result = root.GetSize(); // return directory size;
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static long GetSize(this DirectoryInfo @this)
    {
        return @this.GetFiles("*.*", SearchOption.AllDirectories).Sum(x => x.Length);
    }
}