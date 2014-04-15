// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Xml.Serialization;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that serialize a string to XML.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The string representation of the Xml Serialization.</returns>
        /// <example>
        ///     <code>
        ///           using System.Collections.Generic;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_SerializeXml
        ///               {
        ///                   [TestMethod]
        ///                   public void SerializeXml()
        ///                   {
        ///                       // Type
        ///                       var @this = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
        ///           
        ///                       // Examples
        ///                       string value = @this.SerializeXml(); // Serialize the object into a string.
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;&lt;?xml version=\&quot;1.0\&quot; encoding=\&quot;utf-16\&quot;?&gt;\r\n&lt;ArrayOfString xmlns:xsi=\&quot;http://www.w3.org/2001/XMLSchema-instance\&quot; xmlns:xsd=\&quot;http://www.w3.org/2001/XMLSchema\&quot;&gt;\r\n  &lt;string&gt;Fizz&lt;/string&gt;\r\n  &lt;string&gt;Buzz&lt;/string&gt;\r\n&lt;/ArrayOfString&gt;&quot;, value);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string SerializeXml(this object @this)
        {
            var xmlSerializer = new XmlSerializer(@this.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, @this);
                using (var streamReader = new StringReader(stringWriter.GetStringBuilder().ToString()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}