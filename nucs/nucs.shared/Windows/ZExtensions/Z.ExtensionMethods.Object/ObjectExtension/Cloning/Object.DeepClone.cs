// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Z.ExtensionMethods.Object
{
    public static partial class ObjectExtension
    {
        /// <summary>
        ///     A T extension method that makes a deep copy of '@this' object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>the copied object.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods.Object;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Object_DeepClone
        ///               {
        ///                   [TestMethod]
        ///                   public void DeepClone()
        ///                   {
        ///                       // Type
        ///                       var @this = new TestClass {Value = &quot;Fizz&quot;};
        ///           
        ///                       // Exemples
        ///                       TestClass clone = @this.DeepClone(); // return a deep clone;
        ///           
        ///                       // Unit Test
        ///                       Assert.AreEqual(@this.Value, clone.Value);
        ///                   }
        ///           
        ///                   [Serializable]
        ///                   public class TestClass
        ///                   {
        ///                       public string Value;
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static T DeepClone<T>(this T @this)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, @this);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }
    }
}