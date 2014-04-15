// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class ObjectExtension
{
    /// <summary>
    ///     A T extension method that chains actions.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="action">The action.</param>
    /// <returns>The @this acted on.</returns>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_Chain
    ///               {
    ///                   [TestMethod]
    ///                   public void Chain()
    ///                   {
    ///                       // Type
    ///                       var @this = new List&lt;string&gt;();
    ///           
    ///                       // Exemples
    ///                       @this.Chain(x =&gt; x.Add(&quot;Fizz&quot;))
    ///                            .Chain(x =&gt; x.Add(&quot;Buzz&quot;))
    ///                            .Chain(x =&gt; x.Add(&quot;FizzBuzz&quot;)); // Chain multiple action
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(3, @this.Count);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T Chain<T>(this T @this, Action<T> action)
    {
        action(@this);

        return @this;
    }
}