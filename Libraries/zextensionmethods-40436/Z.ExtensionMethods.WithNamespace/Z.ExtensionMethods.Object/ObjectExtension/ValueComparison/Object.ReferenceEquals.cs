// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     Determines whether the specified  instances are the same instance.
        /// </summary>
        /// <param name="objA">The first object to compare.</param>
        /// <param name="objB">The second object  to compare.</param>
        /// <returns>true if  is the same instance as  or if both are null; otherwise, false.</returns>
        /// <example>
        ///     <code>
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_ReferenceEquals
        ///               {
        ///                   [TestMethod]
        ///                   public void ReferenceEquals()
        ///                   {
        ///                       // Type
        ///                       string @this = null;
        ///           
        ///                       // Examples
        ///                       bool result1 = @this.ReferenceEquals(null); // return true;
        ///                       bool result2 = @this.ReferenceEquals(&quot;&quot;); // return false;
        ///           
        ///                       // Unit Test
        ///                       Assert.IsTrue(result1);
        ///                       Assert.IsFalse(result2);
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static Boolean ReferenceEquals(this System.Object objA, System.Object objB)
        {
            return System.Object.ReferenceEquals(objA, objB);
        }
    }
}