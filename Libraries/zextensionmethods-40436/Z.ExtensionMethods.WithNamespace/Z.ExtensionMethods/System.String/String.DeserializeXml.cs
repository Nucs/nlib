// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Xml.Serialization;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that deserialize an Xml as &lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The desieralize Xml as &lt;T&gt;</returns>
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
        ///               public class System_String_DeserializeXml
        ///               {
        ///                   [TestMethod]
        ///                   public void DeserializeXml()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;&lt;?xml version=\&quot;1.0\&quot; encoding=\&quot;utf-16\&quot;?&gt;\r\n&lt;ArrayOfString xmlns:xsi=\&quot;http://www.w3.org/2001/XMLSchema-instance\&quot; xmlns:xsd=\&quot;http://www.w3.org/2001/XMLSchema\&quot;&gt;\r\n  &lt;string&gt;Fizz&lt;/string&gt;\r\n  &lt;string&gt;Buzz&lt;/string&gt;\r\n&lt;/ArrayOfString&gt;&quot;;
        ///           
        ///                       // Examples
        ///                       var value = @this.DeserializeXml&lt;List&lt;string&gt;&gt;(); // new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Fizz&quot;, value[0]);
        ///           
        ///                       var product = new Product();
        ///                       product.Name = &quot;Apple&quot;;
        ///                       product.Expiry = new DateTime(2008, 12, 28);
        ///                       product.Sizes = new[] {&quot;Small&quot;};
        ///           
        ///                       // Serialize
        ///                       string resultProduct = product.SerializeXml();
        ///           
        ///                       // Deserialize
        ///                       var product2 = resultProduct.DeserializeXml&lt;Product&gt;();
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
        public static T DeserializeXml<T>(this string @this)
        {
            var x = new XmlSerializer(typeof (T));
            var r = new StringReader(@this);

            return (T) x.Deserialize(r);
        }
    }
}