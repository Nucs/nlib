// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that the function result if not null otherwise default value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <returns>The function result if @this is not null otherwise default value.</returns>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IfNotNull
    ///               {
    ///                   [TestMethod]
    ///                   public void IfNotNull()
    ///                   {
    ///                       // Type
    ///                       var values = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
    ///                       List&lt;string&gt; valuesNull = null;
    ///           
    ///                       // Exemples
    ///                       string result1 = values.IfNotNull(x =&gt; x.First(), &quot;FizzBuzz&quot;); // return &quot;Fizz&quot;;
    ///                       string result2 = valuesNull.IfNotNull(x =&gt; x.First(), &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
    ///                       string result3 = valuesNull.IfNotNull(x =&gt; x.First(), () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result1);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result3);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func) where T : class
    {
        return @this != null ? func(@this) : default(TResult);
    }

    /// <summary>
    ///     A T extension method that the function result if not null otherwise default value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The function result if @this is not null otherwise default value.</returns>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IfNotNull
    ///               {
    ///                   [TestMethod]
    ///                   public void IfNotNull()
    ///                   {
    ///                       // Type
    ///                       var values = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
    ///                       List&lt;string&gt; valuesNull = null;
    ///           
    ///                       // Exemples
    ///                       string result1 = values.IfNotNull(x =&gt; x.First(), &quot;FizzBuzz&quot;); // return &quot;Fizz&quot;;
    ///                       string result2 = valuesNull.IfNotNull(x =&gt; x.First(), &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
    ///                       string result3 = valuesNull.IfNotNull(x =&gt; x.First(), () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result1);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result3);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue) where T : class
    {
        return @this != null ? func(@this) : defaultValue;
    }

    /// <summary>
    ///     A T extension method that the function result if not null otherwise default value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="func">The function.</param>
    /// <param name="defaultValueFactory">The default value factory.</param>
    /// <returns>The function result if @this is not null otherwise default value.</returns>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IfNotNull
    ///               {
    ///                   [TestMethod]
    ///                   public void IfNotNull()
    ///                   {
    ///                       // Type
    ///                       var values = new List&lt;string&gt; {&quot;Fizz&quot;, &quot;Buzz&quot;};
    ///                       List&lt;string&gt; valuesNull = null;
    ///           
    ///                       // Exemples
    ///                       string result1 = values.IfNotNull(x =&gt; x.First(), &quot;FizzBuzz&quot;); // return &quot;Fizz&quot;;
    ///                       string result2 = valuesNull.IfNotNull(x =&gt; x.First(), &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;;
    ///                       string result3 = valuesNull.IfNotNull(x =&gt; x.First(), () =&gt; &quot;FizzBuzz&quot;); // return &quot;FizzBuzz&quot;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Fizz&quot;, result1);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result2);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, result3);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static TResult IfNotNull<T, TResult>(this T @this, Func<T, TResult> func, Func<TResult> defaultValueFactory) where T : class
    {
        return @this != null ? func(@this) : defaultValueFactory();
    }
}