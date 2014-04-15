// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Web.Script.Serialization;

namespace Z.ExtensionMethods.Object.Serialization
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that serialize java script.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
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
        ///               public class System_Object_SerializeJavaScript
        ///               {
        ///                   [TestMethod]
        ///                   public void SerializeJavaScript()
        ///                   {
        ///                       // Type
        ///                       var @this = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
        ///           
        ///                       // Examples
        ///                       string result = @this.SerializeJavaScript(); // Serialize the object into a string.
        ///           
        ///                       // Unit Test
        ///                       var result2 = result.DeserializeJavaScript&lt;List&lt;string&gt;&gt;();
        ///                       Assert.AreEqual(2, result2.Count);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result2[0]);
        ///                       Assert.AreEqual(&quot;Buzz&quot;, result2[1]);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string SerializeJavaScript<T>(this T @this)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(@this);
        }
    }
}