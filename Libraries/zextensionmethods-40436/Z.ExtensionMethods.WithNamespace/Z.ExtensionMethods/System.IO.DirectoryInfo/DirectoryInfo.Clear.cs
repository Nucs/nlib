// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.IO;

namespace Z.ExtensionMethods
{
    public static partial class DirectoryInfoExtension
    {
        /// <summary>
        ///     A DirectoryInfo extension method that clears all files and directories in this directory.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
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
        ///               public class System_IO_DirectoryInfo_Clear
        ///               {
        ///                   [TestMethod]
        ///                   public void Clear()
        ///                   {
        ///                       // Type
        ///                       var @this = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_Clear&quot;));
        ///                       Directory.CreateDirectory(@this.FullName);
        ///                       @this.CreateSubdirectory(&quot;FizzBuzz&quot;);
        ///                       int result1 = @this.GetDirectories().Length;
        ///           
        ///                       // Exemples
        ///                       @this.Clear(); // Remove all file and directory in this directory
        ///           
        ///                       // Unit Test
        ///                       int result2 = @this.GetDirectories().Length;
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void Clear(this DirectoryInfo obj)
        {
            Array.ForEach(obj.GetFiles(), x => x.Delete());
            Array.ForEach(obj.GetDirectories(), x => x.Delete(true));
        }
    }
}