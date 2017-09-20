// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using nucs.Windows.FileSystem;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that save the string into a file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="fileName">Filename of the file.</param>
    /// <param name="append">(Optional) if the text should be appended to file file if it's exists.</param>
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
    ///               public class System_String_SaveAs
    ///               {
    ///                   [TestMethod]
    ///                   public void SaveAs()
    ///                   {
    ///                       var fileInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_String_SaveAs.txt&quot;));
    ///                       string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_String_SaveAs2.txt&quot;);
    ///           
    ///                       // Type
    ///                       string @this = &quot;Fizz&quot;;
    ///           
    ///                       // Examples
    ///                       @this.SaveAs(filePath); // Save string in a file.
    ///                       @this.SaveAs(fileInfo); // Save string in a file.
    ///                       @this.SaveAs(fileInfo, true); // Append string to existing file.
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(fileInfo.Exists);
    ///                       Assert.IsTrue(new FileInfo(filePath).Exists);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void SaveAs(this string @this, string fileName, bool append = false, Encoding enc = null)
    {
        var toadd = "";
        if (File.Exists(fileName)) {
            if (append)
                toadd = File.ReadAllText(fileName);
            File.SetAttributes(fileName, FileAttributes.Normal);
            File.Delete(fileName);
        }
        File.WriteAllText(fileName, toadd + @this, enc??Encoding.UTF8);
    }

    /// <summary>
    ///     A string extension method that save the string into a file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="file">The FileInfo.</param>
    /// <param name="append">(Optional) if the text should be appended to file file if it's exists.</param>
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
    ///               public class System_String_SaveAs
    ///               {
    ///                   [TestMethod]
    ///                   public void SaveAs()
    ///                   {
    ///                       var fileInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_String_SaveAs.txt&quot;));
    ///                       string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_String_SaveAs2.txt&quot;);
    ///           
    ///                       // Type
    ///                       string @this = &quot;Fizz&quot;;
    ///           
    ///                       // Examples
    ///                       @this.SaveAs(filePath); // Save string in a file.
    ///                       @this.SaveAs(fileInfo); // Save string in a file.
    ///                       @this.SaveAs(fileInfo, true); // Append string to existing file.
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(fileInfo.Exists);
    ///                       Assert.IsTrue(new FileInfo(filePath).Exists);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void SaveAs(this string @this, FileInfo fileName, bool append = false) {
        var toadd = "";
        if (fileName.Exists) {
            if (append) 
                toadd = File.ReadAllText(fileName.ToString());
            File.SetAttributes(fileName.ToString(), FileAttributes.Normal);
            File.Delete(fileName.ToString());
        }
        File.WriteAllText(fileName.ToString(), @this + toadd, Encoding.UTF8);
    }
}