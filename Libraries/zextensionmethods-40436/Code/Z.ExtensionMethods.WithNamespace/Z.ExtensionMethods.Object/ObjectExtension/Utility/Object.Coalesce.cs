// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that that return the first not null value (including the @this).
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>The first not null value.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_Coalesce
        ///               {
        ///                   [TestMethod]
        ///                   public void Coalesce()
        ///                   {
        ///                       // Type
        ///                       object @thisNull = null;
        ///                       object @thisNotNull = &quot;Fizz&quot;;
        ///           
        ///                       // Exemples
        ///                       object result1 = @thisNull.Coalesce(null, null, &quot;Fizz&quot;, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///                       object result2 = @thisNull.Coalesce(null, &quot;Fizz&quot;, null, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///                       object result3 = @thisNotNull.Coalesce(null, null, null, &quot;Buzz&quot;); // return &quot;Fizz&quot;;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result1);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result2);
        ///                       Assert.AreEqual(&quot;Fizz&quot;, result3);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T Coalesce<T>(this T @this, params T[] values) where T : class
        {
            if (@this != null)
            {
                return @this;
            }

            foreach (T value in values)
            {
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}