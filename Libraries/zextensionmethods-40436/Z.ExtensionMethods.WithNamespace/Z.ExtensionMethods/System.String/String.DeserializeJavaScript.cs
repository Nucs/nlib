// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Web.Script.Serialization;

namespace Z.ExtensionMethods
{
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
        ///               public class System_String_DeserializeJavaScript
        ///               {
        ///                   [TestMethod]
        ///                   public void DeserializeJavaScript()
        ///                   {
        ///                       // Type
        ///                       string @this = &quot;[\&quot;Fizz\&quot;,\&quot;Buzz\&quot;]&quot;;
        ///           
        ///                       // Examples
        ///                       var result = @this.DeserializeJavaScript&lt;List&lt;string&gt;&gt;();
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
        ///                       string resultProduct = product.SerializeJavaScript();
        ///           
        ///                       // Deserialize
        ///                       var product2 = resultProduct.DeserializeJavaScript&lt;Product&gt;();
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
        public static T DeserializeJavaScript<T>(this string @this)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(@this);
        }
    }
}