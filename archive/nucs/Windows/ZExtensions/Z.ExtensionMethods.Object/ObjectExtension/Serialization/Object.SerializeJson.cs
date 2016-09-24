// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).
#if NET4_5|| NET4_0

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Z.ExtensionMethods.Object.Serialization
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that serialize an object to Json.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The Json string.</returns>
        /// <example>
        ///     <code>
        ///           using System.Collections.Generic;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_SerializeJson
        ///               {
        ///                   [TestMethod]
        ///                   public void SerializeJson()
        ///                   {
        ///                       // Type
        ///                       var @this = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
        ///           
        ///                       // Examples
        ///                       string result = @this.SerializeJson(); // Serialize the object into a string.
        ///           
        ///                       // Unit Test
        ///                       var result2 = result.DeserializeJson&lt;List&lt;string&gt;&gt;();
        ///                       Assert.AreEqual(2, result2.Count);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result2[0]);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result2[1]);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string SerializeJson<T>(this T @this)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, @this);
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        ///     A T extension method that serialize an object to Json.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The Json string.</returns>
        /// <example>
        ///     <code>
        ///           using System.Collections.Generic;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_SerializeJson
        ///               {
        ///                   [TestMethod]
        ///                   public void SerializeJson()
        ///                   {
        ///                       // Type
        ///                       var @this = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
        ///           
        ///                       // Examples
        ///                       string result = @this.SerializeJson(); // Serialize the object into a string.
        ///           
        ///                       // Unit Test
        ///                       var result2 = result.DeserializeJson&lt;List&lt;string&gt;&gt;();
        ///                       Assert.AreEqual(2, result2.Count);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result2[0]);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result2[1]);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string SerializeJson<T>(this T @this, Encoding encoding)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, @this);
                return encoding.GetString(memoryStream.ToArray());
            }
        }
    }
}
#endif