// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static partial class StringExtension
{
    /// <summary>
    ///     A string extension method that deserialize a string binary as &lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The desrialize binary as &lt;T&gt;</returns>
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
    ///               public class System_String_DeserializeBinary
    ///               {
    ///                   [TestMethod]
    ///                   public void DeserializeBinary()
    ///                   {
    ///                       // Type
    ///                       var @this = new Dictionary&lt;string, string&gt; {{&quot;Fizz&quot;, &quot;Buzz&quot;}};
    ///                       string s = @this.SerializeBinary();
    ///           
    ///                       // Examples
    ///                       var value = s.DeserializeBinary&lt;Dictionary&lt;string, string&gt;&gt;(); // return new Dictionary&lt;string, string&gt; {{&quot;Fizz&quot;, &quot;Buzz&quot;}};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Buzz&quot;, value[&quot;Fizz&quot;]);
    ///           
    ///                       var product = new Product();
    ///                       product.Name = &quot;Apple&quot;;
    ///                       product.Expiry = new DateTime(2008, 12, 28);
    ///                       product.Sizes = new[] {&quot;Small&quot;};
    ///           
    ///                       // Serialize
    ///                       string resultProduct = product.SerializeBinary();
    ///           
    ///                       // Deserialize
    ///                       var product2 = resultProduct.DeserializeBinary&lt;Product&gt;();
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
    public static T DeserializeBinary<T>(this string @this)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
        {
            var binaryRead = new BinaryFormatter();
            return (T) binaryRead.Deserialize(stream);
        }
    }

    /// <summary>
    ///     A string extension method that deserialize a string binary as &lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>The desrialize binary as &lt;T&gt;</returns>
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
    ///               public class System_String_DeserializeBinary
    ///               {
    ///                   [TestMethod]
    ///                   public void DeserializeBinary()
    ///                   {
    ///                       // Type
    ///                       var @this = new Dictionary&lt;string, string&gt; {{&quot;Fizz&quot;, &quot;Buzz&quot;}};
    ///                       string s = @this.SerializeBinary();
    ///           
    ///                       // Examples
    ///                       var value = s.DeserializeBinary&lt;Dictionary&lt;string, string&gt;&gt;(); // return new Dictionary&lt;string, string&gt; {{&quot;Fizz&quot;, &quot;Buzz&quot;}};
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Buzz&quot;, value[&quot;Fizz&quot;]);
    ///           
    ///                       var product = new Product();
    ///                       product.Name = &quot;Apple&quot;;
    ///                       product.Expiry = new DateTime(2008, 12, 28);
    ///                       product.Sizes = new[] {&quot;Small&quot;};
    ///           
    ///                       // Serialize
    ///                       string resultProduct = product.SerializeBinary();
    ///           
    ///                       // Deserialize
    ///                       var product2 = resultProduct.DeserializeBinary&lt;Product&gt;();
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
    public static T DeserializeBinary<T>(this string @this, Encoding encoding)
    {
        using (var stream = new MemoryStream(encoding.GetBytes(@this)))
        {
            var binaryRead = new BinaryFormatter();
            return (T) binaryRead.Deserialize(stream);
        }
    }
}