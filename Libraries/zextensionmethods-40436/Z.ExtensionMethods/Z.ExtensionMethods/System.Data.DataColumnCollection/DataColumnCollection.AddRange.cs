// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

public static class DataColumnCollectionExtension
{
    /// <summary>
    ///     A DataColumnCollection extension method that adds a range to 'columns'.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="columns">A variable-length parameters list containing columns.</param>
    /// <example>
    ///     <code>
    ///           using System.Data;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_DataColumnCollection_AddRange
    ///               {
    ///                   [TestMethod]
    ///                   public void AddRange()
    ///                   {
    ///                       // Type
    ///                       var @this = new DataTable();
    ///           
    ///                       // Exemples
    ///                       @this.Columns.AddRange(&quot;Column1&quot;, &quot;Column2&quot;, &quot;Column3&quot;); // Add &quot;Column1&quot;, &quot;Column2&quot;, &quot;Column3&quot; to the collection
    ///           
    ///                       // Unit Test
    ///                       Assert.AreEqual(3, @this.Columns.Count);
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void AddRange(this DataColumnCollection @this, params string[] columns)
    {
        foreach (string column in columns)
        {
            @this.Add(column);
        }
    }
}