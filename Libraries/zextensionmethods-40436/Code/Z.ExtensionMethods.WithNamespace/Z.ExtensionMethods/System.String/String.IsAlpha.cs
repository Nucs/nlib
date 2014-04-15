// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Text.RegularExpressions;

namespace Z.ExtensionMethods
{
    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that query if '@this' is Alpha.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if Alpha, false if not.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_String_IsAlpha
        ///               {
        ///                   [TestMethod]
        ///                   public void IsAlpha()
        ///                   {
        ///                       // Type
        ///                       string @thisAlpha = &quot;abc&quot;;
        ///                       string @thisNotAlpha = &quot;abc123&quot;;
        ///           
        ///                       // Examples
        ///                       bool value1 = @thisAlpha.IsAlpha(); // return true;
        ///                       bool value2 = @thisNotAlpha.IsAlpha(); // return false;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsTrue(value1);
        ///                       Assert.IsFalse(value2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static bool IsAlpha(this string @this)
        {
            return !Regex.IsMatch(@this, "[^a-zA-Z]");
        }
    }
}