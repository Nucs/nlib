#if NET_4_5|| NET_4_0
// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that deserialize a Json string to object.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The object deserialized.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.Collections.Generic;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_DeserializeJson
    ///               {
    ///                   [TestMethod]
    ///                   public void DeserializeJson()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;[\&quot;Fizz\&quot;,\&quot;Buzz\&quot;]&quot;;
    ///           
    ///                       // Examples
    ///                       var result = @this.DeserializeJson&lt;List&lt;string&gt;&gt;();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(2, result.Count);
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result[0]);
    ///                       Assert.AreEqual(&quot;Buzz&quot;, result[1]);
    ///           
    ///                       var product = new Product();
    ///                       product.Name = &quot;Apple&quot;;
    ///                       product.Expiry = new DateTime(2008, 12, 28);
    ///                       product.Sizes = new[] {&quot;Small&quot;};
    ///           
    ///                       // Serialize
    ///                       string resultProduct = product.SerializeJson();
    ///           
    ///                       // Deserialize
    ///                       var product2 = resultProduct.DeserializeJson&lt;Product&gt;();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Apple&quot;, product2.Name);
    ///                       Assert.AreEqual(new DateTime(2008, 12, 28).Date, product2.Expiry.Date);
    ///                       Assert.AreEqual(&quot;Small&quot;, product2.Sizes[0]);
    ///                   }
    ///           
    ///                   [Serializable]
    ///                   public class Product
    ///                   {
    ///                       public DateTime Expiry;
    ///                       public string Name;
    ///                       public string[] Sizes;
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T DeserializeJson<T>(this string @this)
    {
        var serializer = new DataContractJsonSerializer(typeof (T));

        using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
        {
            return (T) serializer.ReadObject(stream);
        }
    }

    /// <summary>
    ///     A string extension method that deserialize a Json string to object.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The object deserialized.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.Collections.Generic;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_String_DeserializeJson
    ///               {
    ///                   [TestMethod]
    ///                   public void DeserializeJson()
    ///                   {
    ///                       // Type
    ///                       string @this = &quot;[\&quot;Fizz\&quot;,\&quot;Buzz\&quot;]&quot;;
    ///           
    ///                       // Examples
    ///                       var result = @this.DeserializeJson&lt;List&lt;string&gt;&gt;();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(2, result.Count);
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result[0]);
    ///                       Assert.AreEqual(&quot;Buzz&quot;, result[1]);
    ///           
    ///                       var product = new Product();
    ///                       product.Name = &quot;Apple&quot;;
    ///                       product.Expiry = new DateTime(2008, 12, 28);
    ///                       product.Sizes = new[] {&quot;Small&quot;};
    ///           
    ///                       // Serialize
    ///                       string resultProduct = product.SerializeJson();
    ///           
    ///                       // Deserialize
    ///                       var product2 = resultProduct.DeserializeJson&lt;Product&gt;();
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Apple&quot;, product2.Name);
    ///                       Assert.AreEqual(new DateTime(2008, 12, 28).Date, product2.Expiry.Date);
    ///                       Assert.AreEqual(&quot;Small&quot;, product2.Sizes[0]);
    ///                   }
    ///           
    ///                   [Serializable]
    ///                   public class Product
    ///                   {
    ///                       public DateTime Expiry;
    ///                       public string Name;
    ///                       public string[] Sizes;
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T DeserializeJson<T>(this string @this, Encoding encoding)
    {
        var serializer = new DataContractJsonSerializer(typeof (T));

        using (var stream = new MemoryStream(encoding.GetBytes(@this)))
        {
            return (T) serializer.ReadObject(stream);
        }
    }
}
#endif