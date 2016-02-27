// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.IO.Compression;
using System.Text;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that compress the given string to GZip byte array.
    /// </summary>
    /// <param name="stringToCompress">The stringToCompress to act on.</param>
    /// <returns>The string compressed into a GZip byte array.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_CompressGZip
    ///               {
    ///                   [TestMethod]
    ///                   public void CompressGZip()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;FizzBuzz&quot;;
    ///           
    ///                       // Exemples
    ///                       var result = @this.CompressGZip();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result.DecompressGZip());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static byte[] CompressGZip(this string stringToCompress)
    {
        byte[] stringAsBytes = Encoding.Default.GetBytes(stringToCompress);
        using (var memoryStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                zipStream.Close();
                return (memoryStream.ToArray());
            }
        }
    }

    /// <summary>
    ///     A string extension method that compress the given string to GZip byte array.
    /// </summary>
    /// <param name="stringToCompress">The stringToCompress to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The string compressed into a GZip byte array.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_CompressGZip
    ///               {
    ///                   [TestMethod]
    ///                   public void CompressGZip()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;FizzBuzz&quot;;
    ///           
    ///                       // Exemples
    ///                       var result = @this.CompressGZip();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result.DecompressGZip());
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static byte[] CompressGZip(this string stringToCompress, Encoding encoding)
    {
        byte[] stringAsBytes = encoding.GetBytes(stringToCompress);
        using (var memoryStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                zipStream.Close();
                return (memoryStream.ToArray());
            }
        }
    }
}