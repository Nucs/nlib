// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an int.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToInt32OrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToInt32OrDefault()
        ///                   {
        ///                       // Type
        ///                       string @thisValid = &quot;32&quot;;
        ///                       string @thisInvalid = &quot;FizzBuzz&quot;;
        ///           
        ///                       // Exemples
        ///                       int result1 = @thisValid.ToInt32OrDefault(); // return 32;
        ///                       int result2 = @thisInvalid.ToInt32OrDefault(); // return 0;
        ///                       int result3 = @thisInvalid.ToInt32OrDefault(-1); // return -1;
        ///                       int result4 = @thisInvalid.ToInt32OrDefault(() =&gt; -2); // return -2;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(32, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(-1, result3);
        ///                       Assert.AreEqual(-2, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int ToInt32OrDefault(this object @this)
        {
            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return default(int);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an int.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToInt32OrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToInt32OrDefault()
        ///                   {
        ///                       // Type
        ///                       string @thisValid = &quot;32&quot;;
        ///                       string @thisInvalid = &quot;FizzBuzz&quot;;
        ///           
        ///                       // Exemples
        ///                       int result1 = @thisValid.ToInt32OrDefault(); // return 32;
        ///                       int result2 = @thisInvalid.ToInt32OrDefault(); // return 0;
        ///                       int result3 = @thisInvalid.ToInt32OrDefault(-1); // return -1;
        ///                       int result4 = @thisInvalid.ToInt32OrDefault(() =&gt; -2); // return -2;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(32, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(-1, result3);
        ///                       Assert.AreEqual(-2, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int ToInt32OrDefault(this object @this, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to an int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an int.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToInt32OrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToInt32OrDefault()
        ///                   {
        ///                       // Type
        ///                       string @thisValid = &quot;32&quot;;
        ///                       string @thisInvalid = &quot;FizzBuzz&quot;;
        ///           
        ///                       // Exemples
        ///                       int result1 = @thisValid.ToInt32OrDefault(); // return 32;
        ///                       int result2 = @thisInvalid.ToInt32OrDefault(); // return 0;
        ///                       int result3 = @thisInvalid.ToInt32OrDefault(-1); // return -1;
        ///                       int result4 = @thisInvalid.ToInt32OrDefault(() =&gt; -2); // return -2;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(32, result1);
        ///                       Assert.AreEqual(0, result2);
        ///                       Assert.AreEqual(-1, result3);
        ///                       Assert.AreEqual(-2, result4);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int ToInt32OrDefault(this object @this, Func<int> defaultValueFactory)
        {
            try
            {
                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }
    }
}