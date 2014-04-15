// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).


public static partial class ObjectExtension
{
    /// <summary>
    ///     An object extension method that query if '@this' is valid int.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if valid int, false if not.</returns>
    /// <example>
    ///     <code>
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods.Object;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Object_IsValidInt32
    ///               {
    ///                   [TestMethod]
    ///                   public void IsValidInt32()
    ///                   {
    ///                       // Exemples
    ///                       object nullValue = null;
    ///                       bool result1 = nullValue.IsValidInt32(); // return true;
    ///                       bool result2 = &quot;12345&quot;.IsValidInt32(); // return true;
    ///                       bool result3 = &quot;1.32&quot;.IsValidInt32(); // return false;
    ///                       bool result4 = &quot;ABC&quot;.IsValidInt32(); // return false;
    ///           
    ///                       // Unit Test
    ///                       Assert.IsTrue(result1);
    ///                       Assert.IsTrue(result2);
    ///                       Assert.IsFalse(result3);
    ///                       Assert.IsFalse(result4);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool IsValidInt32(this object @this)
    {
        if (@this == null)
        {
            return true;
        }

        int result;
        return int.TryParse(@this.ToString(), out result);
    }
}