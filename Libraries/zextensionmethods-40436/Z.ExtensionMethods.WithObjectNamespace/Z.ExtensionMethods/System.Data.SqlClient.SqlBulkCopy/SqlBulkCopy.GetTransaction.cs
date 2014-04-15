// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data.SqlClient;
using System.Reflection;

public static partial class SqlBulkCopyExtension
{
    /// <summary>
    ///     A SqlBulkCopy extension method that return the SqlTransaction from the SqlBulkCopy.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The SqlTransaction from the SqlBulkCopy.</returns>
    /// <example>
    ///     <code>
    ///           using System.Data.SqlClient;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_GetTransaction
    ///               {
    ///                   [TestMethod]
    ///                   public void GetTransaction()
    ///                   {
    ///                       // Examples
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           conn.Open();
    ///                           SqlTransaction trans = conn.BeginTransaction();
    ///           
    ///                           using (var @this = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
    ///                           {
    ///                               SqlTransaction result = @this.GetTransaction();
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(trans, result);
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static SqlTransaction GetTransaction(this SqlBulkCopy @this)
    {
        Type type = @this.GetType();
        FieldInfo field = type.GetField("_externalTransaction", BindingFlags.NonPublic | BindingFlags.Instance);
        return field.GetValue(@this) as SqlTransaction;
    }
}