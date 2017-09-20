// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Security.Cryptography;
using System.Text;

public static partial class StreamExtension
{
    /// <summary>
    ///     A Stream extension method that converts the @this to a md 5 hash.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a string.</returns>
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
    ///               public class System_IO_Stream_ToMD5Hash
    ///               {
    ///                   [TestMethod]
    ///                   public void ToMD5Hash()
    ///                   {
    ///                       var fileInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ToMD5Hash.txt&quot;));
    ///           
    ///                       // Examples
    ///                       string result;
    ///           
    ///                       using (FileStream @this = fileInfo.Create())
    ///                       {
    ///                           @this.WriteByte(0);
    ///                           result = @this.ToMD5Hash(); // return &quot;D41D8CD98F00B204E9800998ECF8427E&quot;;
    ///                       }
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;D41D8CD98F00B204E9800998ECF8427E&quot;, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ToMD5Hash(this Stream @this)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(@this);
            var sb = new StringBuilder();
            foreach (byte bytes in hashBytes)
            {
                sb.Append(bytes.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}