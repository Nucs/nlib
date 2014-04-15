// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.ComponentModel;
using System.Linq;

public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that gets description attribute.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>The description attribute.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               public enum TestEnum
    ///               {
    ///                   [System.ComponentModel.Description(&quot;Test Description&quot;)] Test
    ///               }
    ///           
    ///               [TestClass]
    ///               public class System_Object_GetCustomAttributeDescription
    ///               {
    ///                   /// &lt;summary&gt;
    ///                   ///     System.String GetCustomAttributeDescription(System.Object)
    ///                   /// &lt;/summary&gt;
    ///                   [TestMethod]
    ///                   public void GetCustomAttributeDescription()
    ///                   {
    ///                       // Type
    ///                       var @this = TestEnum.Test;
    ///           
    ///                       // Exemples
    ///                       string result = @this.GetCustomAttributeDescription(); // return &quot;Test Description&quot;;
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(&quot;Test Description&quot;, result);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static string GetCustomAttributeDescription(this object value)
    {
        var attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        return attr.Description;
    }
}