// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).


public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that cast anonymous type to the specified type.
    /// </summary>
    /// <typeparam name="T">Generic type parameter. The specified type.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The object as the specified type.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_As
    ///               {
    ///                   [TestMethod]
    ///                   public void As()
    ///                   {
    ///                       // Type
    ///                       var intObject = (object) 13;
    ///                       var stringObject = (object) &quot;FizzBuzz&quot;;
    ///                       var arrayObject = (object) new[] {&quot;Fizz&quot;, &quot;Buzz&quot;};
    ///           
    ///                       // Exemples
    ///                       var intValue = intObject.As&lt;int&gt;(); // return 13;
    ///                       var stringValue = stringObject.As&lt;string&gt;(); // return &quot;FizzBuzz&quot;;
    ///                       int arrayCount = arrayObject.As&lt;string[]&gt;().Length; // return 2;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(13, intValue);
    ///                       Assert.AreEqual(&quot;FizzBuzz&quot;, stringValue);
    ///                       Assert.AreEqual(2, arrayCount);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T As<T>(this object @this)
    {
        return (T) @this;
    }
}