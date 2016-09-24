// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;
using System.Linq;
using MoreLinq;

public static partial class DirectoryInfoExtension
{
    /// <summary>
    ///     A DirectoryInfo extension method that deletes the files where.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="predicate">The predicate.</param>
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
    ///               public class System_IO_DirectoryInfo_DeleteFilesWhere
    ///               {
    ///                   [TestMethod]
    ///                   public void DeleteFilesWhere()
    ///                   {
    ///                       // Type
    ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_DeleteFilesWhere&quot;));
    ///                       Directory.CreateDirectory(root.FullName);
    ///           
    ///                       var file1 = new FileInfo(Path.Combine(root.FullName, &quot;test.txt&quot;));
    ///                       var file2 = new FileInfo(Path.Combine(root.FullName, &quot;test.cs&quot;));
    ///                       var file3 = new FileInfo(Path.Combine(root.FullName, &quot;test.asp&quot;));
    ///                       using (FileStream f = file1.Create())
    ///                       {
    ///                       }
    ///                       using (FileStream f = file2.Create())
    ///                       {
    ///                       }
    ///                       using (FileStream f = file3.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Exemples
    ///                       root.DeleteFilesWhere(x =&gt; x.Extension == &quot;.cs&quot;);
    ///                       FileInfo[] result = root.GetFiles();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(2, result.Length);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void DeleteFilesWhere(this DirectoryInfo obj, Func<FileInfo, bool> predicate)
    {
        obj.GetFiles().Where(predicate).ForEach(x => x.Delete());
    }

    /// <summary>
    ///     A DirectoryInfo extension method that deletes the files where.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="searchOption">The search option.</param>
    /// <param name="predicate">The predicate.</param>
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
    ///               public class System_IO_DirectoryInfo_DeleteFilesWhere
    ///               {
    ///                   [TestMethod]
    ///                   public void DeleteFilesWhere()
    ///                   {
    ///                       // Type
    ///                       var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;System_IO_DirectoryInfo_DeleteFilesWhere&quot;));
    ///                       Directory.CreateDirectory(root.FullName);
    ///           
    ///                       var file1 = new FileInfo(Path.Combine(root.FullName, &quot;test.txt&quot;));
    ///                       var file2 = new FileInfo(Path.Combine(root.FullName, &quot;test.cs&quot;));
    ///                       var file3 = new FileInfo(Path.Combine(root.FullName, &quot;test.asp&quot;));
    ///                       using (FileStream f = file1.Create())
    ///                       {
    ///                       }
    ///                       using (FileStream f = file2.Create())
    ///                       {
    ///                       }
    ///                       using (FileStream f = file3.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Exemples
    ///                       root.DeleteFilesWhere(x =&gt; x.Extension == &quot;.cs&quot;);
    ///                       FileInfo[] result = root.GetFiles();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(2, result.Length);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void DeleteFilesWhere(this DirectoryInfo obj, SearchOption searchOption, Func<FileInfo, bool> predicate)
    {
        obj.GetFiles("*.*", searchOption).Where(predicate).ForEach(x => x.Delete());
    }
}