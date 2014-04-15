// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

public static partial class RandomExtension
{
    /// <summary>
    ///     A Random extension method that flip a coin toss.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true 50% of time, otherwise false.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Random_CoinToss
    ///               {
    ///                   [TestMethod]
    ///                   public void CoinToss()
    ///                   {
    ///                       // Type
    ///                       var @this = new Random();
    ///           
    ///                       // Examples
    ///                       bool value = @this.CoinToss(); // return true or false at random.
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static bool CoinToss(this Random @this)
    {
        return @this.Next(2) == 0;
    }
}