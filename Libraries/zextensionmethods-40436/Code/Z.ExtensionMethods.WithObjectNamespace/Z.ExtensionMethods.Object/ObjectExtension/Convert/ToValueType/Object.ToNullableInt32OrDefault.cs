// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     An object extension method that converts this object to a nullable int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an int?</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToNullableInt32OrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToNullableInt32OrDefault()
        ///                   {
        ///                       // Type
        ///                       string @thisValid = &quot;32&quot;;
        ///                       string @thisInvalid = &quot;FizzBuzz&quot;;
        ///                       string @thisNull = null;
        ///           
        ///                       // Exemples
        ///                       int? result1 = @thisValid.ToNullableInt32OrDefault(); // return 32;
        ///                       int? result2 = @thisInvalid.ToNullableInt32OrDefault(); // return 0;
        ///                       int? result3 = @thisInvalid.ToNullableInt32OrDefault(-1); // return -1;
        ///                       int? result4 = @thisInvalid.ToNullableInt32OrDefault(() =&gt; -2); // return -2;
        ///                       int? result5 = @thisNull.ToNullableInt32OrDefault(); // return null;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(32, result1.Value);
        ///                       Assert.AreEqual(0, result2.Value);
        ///                       Assert.AreEqual(-1, result3.Value);
        ///                       Assert.AreEqual(-2, result4.Value);
        ///                       Assert.IsFalse(result5.HasValue);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int? ToNullableInt32OrDefault(this object @this)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return default(int);
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to an int?</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToNullableInt32OrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToNullableInt32OrDefault()
        ///                   {
        ///                       // Type
        ///                       string @thisValid = &quot;32&quot;;
        ///                       string @thisInvalid = &quot;FizzBuzz&quot;;
        ///                       string @thisNull = null;
        ///           
        ///                       // Exemples
        ///                       int? result1 = @thisValid.ToNullableInt32OrDefault(); // return 32;
        ///                       int? result2 = @thisInvalid.ToNullableInt32OrDefault(); // return 0;
        ///                       int? result3 = @thisInvalid.ToNullableInt32OrDefault(-1); // return -1;
        ///                       int? result4 = @thisInvalid.ToNullableInt32OrDefault(() =&gt; -2); // return -2;
        ///                       int? result5 = @thisNull.ToNullableInt32OrDefault(); // return null;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(32, result1.Value);
        ///                       Assert.AreEqual(0, result2.Value);
        ///                       Assert.AreEqual(-1, result3.Value);
        ///                       Assert.AreEqual(-2, result4.Value);
        ///                       Assert.IsFalse(result5.HasValue);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int? ToNullableInt32OrDefault(this object @this, int? defaultValue)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts this object to a nullable int 32 or default.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to an int?</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ToNullableInt32OrDefault
        ///               {
        ///                   [TestMethod]
        ///                   public void ToNullableInt32OrDefault()
        ///                   {
        ///                       // Type
        ///                       string @thisValid = &quot;32&quot;;
        ///                       string @thisInvalid = &quot;FizzBuzz&quot;;
        ///                       string @thisNull = null;
        ///           
        ///                       // Exemples
        ///                       int? result1 = @thisValid.ToNullableInt32OrDefault(); // return 32;
        ///                       int? result2 = @thisInvalid.ToNullableInt32OrDefault(); // return 0;
        ///                       int? result3 = @thisInvalid.ToNullableInt32OrDefault(-1); // return -1;
        ///                       int? result4 = @thisInvalid.ToNullableInt32OrDefault(() =&gt; -2); // return -2;
        ///                       int? result5 = @thisNull.ToNullableInt32OrDefault(); // return null;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(32, result1.Value);
        ///                       Assert.AreEqual(0, result2.Value);
        ///                       Assert.AreEqual(-1, result3.Value);
        ///                       Assert.AreEqual(-2, result4.Value);
        ///                       Assert.IsFalse(result5.HasValue);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static int? ToNullableInt32OrDefault(this object @this, Func<int?> defaultValueFactory)
        {
            try
            {
                if (@this == null || @this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ToInt32(@this);
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }
    }
}