// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Text;

public static partial class FileInfoExtension
{
    /// <summary>
    ///     A FileInfo extension method that reads the file to the end.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>
    ///     The rest of the stream as a string, from the current position to the end. If the current position is at the
    ///     end of the stream, returns an empty string ("").
    /// </returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.IO;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_FileInfo_ReadToEnd
    ///               {
    ///                   [TestMethod]
    ///                   public void ReadToEnd()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadToEnd.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                           byte[] byteToWrites = Encoding.Default.GetBytes(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///                           stream.Write(byteToWrites, 0, byteToWrites.Length);
    ///                       }
    ///           
    ///                       // Examples
    ///                       string result = @this.ReadToEnd(); // return &quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ReadToEnd(this FileInfo @this)
    {
        using (FileStream stream = File.Open(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (var reader = new StreamReader(stream, Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }
    }

    /// <summary>
    ///     A FileInfo extension method that reads the file to the end.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="position">The position.</param>
    /// <returns>
    ///     The rest of the stream as a string, from the current position to the end. If the current position is at the
    ///     end of the stream, returns an empty string ("").
    /// </returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.IO;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_FileInfo_ReadToEnd
    ///               {
    ///                   [TestMethod]
    ///                   public void ReadToEnd()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadToEnd.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                           byte[] byteToWrites = Encoding.Default.GetBytes(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///                           stream.Write(byteToWrites, 0, byteToWrites.Length);
    ///                       }
    ///           
    ///                       // Examples
    ///                       string result = @this.ReadToEnd(); // return &quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ReadToEnd(this FileInfo @this, long position)
    {
        using (FileStream stream = File.Open(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            stream.Position = position;

            using (var reader = new StreamReader(stream, Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }
    }

    /// <summary>
    ///     A FileInfo extension method that reads the file to the end.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>
    ///     The rest of the stream as a string, from the current position to the end. If the current position is at the
    ///     end of the stream, returns an empty string ("").
    /// </returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.IO;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_FileInfo_ReadToEnd
    ///               {
    ///                   [TestMethod]
    ///                   public void ReadToEnd()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadToEnd.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                           byte[] byteToWrites = Encoding.Default.GetBytes(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///                           stream.Write(byteToWrites, 0, byteToWrites.Length);
    ///                       }
    ///           
    ///                       // Examples
    ///                       string result = @this.ReadToEnd(); // return &quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ReadToEnd(this FileInfo @this, Encoding encoding)
    {
        using (FileStream stream = File.Open(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }

    /// <summary>
    ///     A FileInfo extension method that reads the file to the end.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="position">The position.</param>
    /// <returns>
    ///     The rest of the stream as a string, from the current position to the end. If the current position is at the
    ///     end of the stream, returns an empty string ("").
    /// </returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.IO;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_IO_FileInfo_ReadToEnd
    ///               {
    ///                   [TestMethod]
    ///                   public void ReadToEnd()
    ///                   {
    ///                       // Type
    ///                       var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, &quot;Examples_System_IO_FileInfo_ReadToEnd.txt&quot;));
    ///           
    ///                       // Intialization
    ///                       using (FileStream stream = @this.Create())
    ///                       {
    ///                           byte[] byteToWrites = Encoding.Default.GetBytes(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;);
    ///                           stream.Write(byteToWrites, 0, byteToWrites.Length);
    ///                       }
    ///           
    ///                       // Examples
    ///                       string result = @this.ReadToEnd(); // return &quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot; + Environment.NewLine + &quot;Buzz&quot;, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string ReadToEnd(this FileInfo @this, Encoding encoding, long position)
    {
        using (FileStream stream = File.Open(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            stream.Position = position;

            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}