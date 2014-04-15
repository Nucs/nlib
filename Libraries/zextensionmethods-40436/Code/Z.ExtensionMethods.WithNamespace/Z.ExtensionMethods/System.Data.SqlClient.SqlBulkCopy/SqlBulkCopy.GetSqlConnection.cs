// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Z.ExtensionMethods
{
    public static partial class SqlBulkCopyExtension
    {
        /// <summary>
        ///     A SqlBulkCopy extension method that return the SqlConnection from the SqlBulkCopy.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The SqlConnection from the SqlBulkCopy.</returns>
        /// <example>
        ///     <code>
        ///           using System.Data.SqlClient;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Data_SqlClient_SqlBulkCopy_GetSqlConnection
        ///               {
        ///                   [TestMethod]
        ///                   public void GetSqlConnection()
        ///                   {
        ///                       // Examples
        ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
        ///                       {
        ///                           using (var @this = new SqlBulkCopy(conn))
        ///                           {
        ///                               SqlConnection result = @this.GetSqlConnection();
        ///           
        ///                               // Unit Test
        ///                               Assert.AreEqual(conn, result);
        ///                           }
        ///                       }
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static SqlConnection GetSqlConnection(this SqlBulkCopy @this)
        {
            Type type = @this.GetType();
            FieldInfo field = type.GetField("_connection", BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(@this) as SqlConnection;
        }
    }
}