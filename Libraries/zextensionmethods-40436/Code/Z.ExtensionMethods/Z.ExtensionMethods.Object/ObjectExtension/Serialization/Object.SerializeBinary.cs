// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that serialize an object to binary.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A string.</returns>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.IO;
    ///           using System.Runtime.Serialization.Formatters.Binary;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_SerializeBinary
    ///               {
    ///                   [TestMethod]
    ///                   public void SerializeBinary()
    ///                   {
    ///                       // Type
    ///                       var @this = new Dictionary&lt;string, string&gt; {{&quot;Fizz&quot;, &quot;Buzz&quot;}};
    ///           
    ///                       // Examples
    ///                       string result = @this.SerializeBinary(); // Serialize the object into a string.
    ///           
    ///                       // Unit Test
    ///                       using (var stream = new MemoryStream(Encoding.Default.GetBytes(result)))
    ///                       {
    ///                           var binaryRead = new BinaryFormatter();
    ///                           var dict = (Dictionary&lt;string, string&gt;) binaryRead.Deserialize(stream);
    ///                           Assert.IsTrue(dict.ContainsKey(&quot;Fizz&quot;));
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string SerializeBinary<T>(this T @this)
    {
        var binaryWrite = new BinaryFormatter();

        using (var memoryStream = new MemoryStream())
        {
            binaryWrite.Serialize(memoryStream, @this);
            return Encoding.Default.GetString(memoryStream.ToArray());
        }
    }

    /// <summary>
    ///     An object extension method that serialize an object to binary.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>A string.</returns>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.IO;
    ///           using System.Runtime.Serialization.Formatters.Binary;
    ///           using System.Text;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_SerializeBinary
    ///               {
    ///                   [TestMethod]
    ///                   public void SerializeBinary()
    ///                   {
    ///                       // Type
    ///                       var @this = new Dictionary&lt;string, string&gt; {{&quot;Fizz&quot;, &quot;Buzz&quot;}};
    ///           
    ///                       // Examples
    ///                       string result = @this.SerializeBinary(); // Serialize the object into a string.
    ///           
    ///                       // Unit Test
    ///                       using (var stream = new MemoryStream(Encoding.Default.GetBytes(result)))
    ///                       {
    ///                           var binaryRead = new BinaryFormatter();
    ///                           var dict = (Dictionary&lt;string, string&gt;) binaryRead.Deserialize(stream);
    ///                           Assert.IsTrue(dict.ContainsKey(&quot;Fizz&quot;));
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string SerializeBinary<T>(this T @this, Encoding encoding)
    {
        var binaryWrite = new BinaryFormatter();

        using (var memoryStream = new MemoryStream())
        {
            binaryWrite.Serialize(memoryStream, @this);
            return encoding.GetString(memoryStream.ToArray());
        }
    }
}