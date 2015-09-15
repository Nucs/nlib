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
    ///     A FileInfo extension method that appends all lines.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="contents">The contents.</param>
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
    ///               public class System_IO_FileInfo_AppendAllLines
    ///               {
    ///                   [TestMethod]
    ///                   public void AppendAllLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_AppendAllLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.AppendAllLines(new[] {&quot;Fizz&quot;, &quot;Buzz&quot;});
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot; + Environment.NewLine, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void AppendAllLines(this FileInfo @this, IEnumerable<String> contents)
    {
#if (NET_3_5 || NET_3_0 || NET_2_0)

        File.AppendAllText(@this.FullName, contents.StringJoin(Environment.NewLine));
#else
        File.AppendAllLines(@this.FullName, contents);
#endif
    }

    /// <summary>
    ///     A FileInfo extension method that appends all lines.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="contents">The contents.</param>
    /// <param name="encoding">The encoding.</param>
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
    ///               public class System_IO_FileInfo_AppendAllLines
    ///               {
    ///                   [TestMethod]
    ///                   public void AppendAllLines()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_AppendAllLines.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.AppendAllLines(new[] {&quot;Fizz&quot;, &quot;Buzz&quot;});
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot; + Environment.NewLine, @this.ReadToEnd());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void AppendAllLines(this FileInfo @this, IEnumerable<String> contents, Encoding encoding)
    {
        File.AppendAllText(@this.FullName, contents.StringJoin(Environment.NewLine), encoding);
    }
}