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
        ///     Returns the file name of the specified @this string without the extension.
        /// </summary>
        /// <param name="this">The @this of the file.</param>
        /// <returns>
        ///     The string returned by <see cref="M:System.IO.Path.GetFileName(System.String)" />, minus the last period (.)
        ///     and all characters following it.
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
        ///               public class System_IO_FileInfo_GetFileNameWithoutExtension
        ///               {
        ///                   [TestMethod]
        ///                   public void GetFileNameWithoutExtension()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;DirectoryInfo_GetDirectoryName&quot;, &quot;CreateDirectory.txt&quot;));
        ///           
        ///                       // Examples
        ///                       string result = @this.GetFileNameWithoutExtension(); // return &quot;CreateDirectory&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;CreateDirectory&quot;, result);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static String GetFileNameWithoutExtension(this FileInfo @this)
        {
            return Path.GetFileNameWithoutExtension(@this.FullName);
        }
    }
}