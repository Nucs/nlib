// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that converts the @this to string or return an empty string if the value is null.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a string or empty if the value is null.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToStringSafe
        ///               {
        ///                   [TestMethod]
        ///                   public void ToStringSafe()
        ///                   {
        ///                       // Type
        ///                       int @thisValue = 1;
        ///                       string @thisNull = null;
        ///           
        ///                       // Exemples
        ///                       string result1 = @thisValue.ToStringSafe(); // return &quot;1&quot;;
        ///                       string result2 = @thisNull.ToStringSafe(); // return &quot;&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(result1, &quot;1&quot;);
        ///                       Assert.AreEqual(result2, &quot;&quot;);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static string ToStringSafe(this object @this)
        {
            return @this == null ? "" : @this.ToString();
        }
    }
}