// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.IO.Compression;

namespace Z.ExtensionMethods
{
    public static partial class FileInfoExtension
    {
        /// <summary>
        ///     A FileInfo extension method that extracts the g zip.
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
        ///               public class System_IO_FileInfo_ExtractGZip
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractGZip()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip.txt&quot;));
        ///                       var output = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip.gz&quot;));
        ///                       var outputExtract = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip_Example.txt&quot;));
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                       }
        ///           
        ///                       // Examples
        ///                       @this.CreateGZip(output);
        ///                      output.ExtractGZip(outputExtract);
        ///           
        ///                       // Unit Test
        ///                      Assert.IsTrue(outputExtract.Exists);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static void ExtractGZip(this FileInfo @this)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                string newFileName = Path.GetFileNameWithoutExtension(@this.FullName);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that extracts the g zip.
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
        ///               public class System_IO_FileInfo_ExtractGZip
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractGZip()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip.txt&quot;));
        ///                       var output = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip.gz&quot;));
        ///                       var outputExtract = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip_Example.txt&quot;));
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                       }
        ///           
        ///                       // Examples
        ///                       @this.CreateGZip(output);
        ///                      output.ExtractGZip(outputExtract);
        ///           
        ///                       // Unit Test
        ///                      Assert.IsTrue(outputExtract.Exists);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        /// <param name="destination">Destination for the.</param>
        public static void ExtractGZip(this FileInfo @this, string destination)
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
        ///     A FileInfo extension method that extracts the g zip.
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
        ///               public class System_IO_FileInfo_ExtractGZip
        ///               {
        ///                   [TestMethod]
        ///                   public void ExtractGZip()
        ///                   {
        ///                       // Type
        ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip.txt&quot;));
        ///                       var output = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip.gz&quot;));
        ///                       var outputExtract = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ExtractGZip_Example.txt&quot;));
        ///           
        ///                       // Intialization
        ///                       using (FileStream stream = @this.Create())
        ///                       {
        ///                       }
        ///           
        ///                       // Examples
        ///                       @this.CreateGZip(output);
        ///                      output.ExtractGZip(outputExtract);
        ///           
        ///                       // Unit Test
        ///                      Assert.IsTrue(outputExtract.Exists);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        /// <param name="destination">Destination for the.</param>
        public static void ExtractGZip(this FileInfo @this, FileInfo destination)
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
}