// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Reflection;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that gets a property.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="name">The name.</param>
        /// <returns>The property.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_GetPropertyValue
        ///               {
        ///                   [TestMethod]
        ///                   public void GetPropertyValue()
        ///                   {
        ///                       // Type
        ///                       var @this = new TestClass {PublicProperty = 1};
        ///                       TestClass.InternaStaticProperty = 2;
        ///           
        ///                       // Exemples
        ///                       object result1 = @this.GetPropertyValue(&quot;PublicProperty&quot;); // return 1;
        ///                       object result2 = @this.GetPropertyValue(&quot;InternaStaticProperty&quot;); // return 2;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(2, result2);
        ///                   }
        ///           
        ///                   public class TestClass
        ///                   {
        ///                       internal static int InternaStaticProperty { get; set; }
        ///                       public int PublicProperty { get; set; }
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static PropertyInfo GetProperty<T>(this T @this, string name)
        {
            return @this.GetType().GetProperty(name);
        }

        /// <summary>
        ///     A T extension method that gets a property.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="name">The name.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <returns>The property.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_GetPropertyValue
        ///               {
        ///                   [TestMethod]
        ///                   public void GetPropertyValue()
        ///                   {
        ///                       // Type
        ///                       var @this = new TestClass {PublicProperty = 1};
        ///                       TestClass.InternaStaticProperty = 2;
        ///           
        ///                       // Exemples
        ///                       object result1 = @this.GetPropertyValue(&quot;PublicProperty&quot;); // return 1;
        ///                       object result2 = @this.GetPropertyValue(&quot;InternaStaticProperty&quot;); // return 2;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(1, result1);
        ///                       Assert.AreEqual(2, result2);
        ///                   }
        ///           
        ///                   public class TestClass
        ///                   {
        ///                       internal static int InternaStaticProperty { get; set; }
        ///                       public int PublicProperty { get; set; }
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static PropertyInfo GetProperty<T>(this T @this, string name, BindingFlags bindingAttr)
        {
            return @this.GetType().GetProperty(name, bindingAttr);
        }
    }
}