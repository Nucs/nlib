// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).
#if !(NET35 || NET3 || NET2)

using System.IO;
using System.IO.Compression;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     A FileInfo extension method that creates a zip file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
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
    ///               public class System_IO_FileInfo_CreateGZip
    ///               {
    ///                   [TestMethod]
    ///                   public void CreateGZip()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_CreateGZip.txt&quot;));
    ///                       var output = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_CreateGZip.gz&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.CreateGZip(output);
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(output.Exists);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void CreateGZip(this FileInfo @this)
    {
        using (FileStream originalFileStream = @this.OpenRead())
        {
            using (FileStream compressedFileStream = File.Create(@this.FullName + ".gz"))
            {
                using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }
    }

    /// <summary>
    ///     A FileInfo extension method that creates a zip file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
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
    ///               public class System_IO_FileInfo_CreateGZip
    ///               {
    ///                   [TestMethod]
    ///                   public void CreateGZip()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_CreateGZip.txt&quot;));
    ///                       var output = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_CreateGZip.gz&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.CreateGZip(output);
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(output.Exists);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    /// <param name="destination">Destination for the zip.</param>
    public static void CreateGZip(this FileInfo @this, string destination)
    {
        using (FileStream originalFileStream = @this.OpenRead())
        {
            using (FileStream compressedFileStream = File.Create(destination))
            {
                using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }
    }

    /// <summary>
    ///     A FileInfo extension method that creates a zip file.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
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
    ///               public class System_IO_FileInfo_CreateGZip
    ///               {
    ///                   [TestMethod]
    ///                   public void CreateGZip()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_CreateGZip.txt&quot;));
    ///                       var output = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_CreateGZip.gz&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                       }
    ///           
    ///                       // Examples
    ///                       @this.CreateGZip(output);
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(output.Exists);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    /// <param name="destination">Destination for the zip.</param>
    public static void CreateGZip(this FileInfo @this, FileInfo destination)
    {
        using (FileStream originalFileStream = @this.OpenRead())
        {
            using (FileStream compressedFileStream = File.Create(destination.FullName))
            {
                using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }
    }
}
#endif