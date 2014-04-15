// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Z.Utility;

public static partial class SqlBulkCopyExtension
{
    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="drs">The drs.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, DataRow[] drs)
    {
        if (drs.Length > 0)
        {
            var bulkOperation = new SqlBulkOperation
                {
                    SqlBulkCopy = obj,
                    DataSource = drs
                };
            bulkOperation.BulkUpdate();
        }
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="drs">The drs.</param>
    /// <param name="primaryKeyColumnNames">List of names of the primary key columns.</param>
    /// ###
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, DataRow[] drs, IEnumerable<string> primaryKeyColumnNames)
    {
        if (drs.Length > 0)
        {
            var bulkOperation = new SqlBulkOperation
                {
                    SqlBulkCopy = obj,
                    DataSource = drs,
                    PrimaryKeys = primaryKeyColumnNames.ToList()
                };
            bulkOperation.BulkUpdate();
        }
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="dt">The dt.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            var bulkOperation = new SqlBulkOperation
                {
                    SqlBulkCopy = obj,
                    DataSource = dt
                };
            bulkOperation.BulkUpdate();
        }
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="dt">The dt.</param>
    /// <param name="primaryKeyColumnNames">List of names of the primary key columns.</param>
    /// ###
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, DataTable dt, IEnumerable<string> primaryKeyColumnNames)
    {
        if (dt.Rows.Count > 0)
        {
            var bulkOperation = new SqlBulkOperation
                {
                    SqlBulkCopy = obj,
                    DataSource = dt,
                    PrimaryKeys = primaryKeyColumnNames.ToList()
                };
            bulkOperation.BulkUpdate();
        }
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="dt">The dt.</param>
    /// <param name="state">The state.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, DataTable dt, DataRowState state)
    {
        if (dt.Rows.Count > 0)
        {
            var bulkOperation = new SqlBulkOperation
                {
                    SqlBulkCopy = obj,
                    DataSource = dt,
                    DataRowState = state
                };
            bulkOperation.BulkUpdate();
        }
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="dt">The dt.</param>
    /// <param name="state">The state.</param>
    /// <param name="primaryKeyColumnNames">List of names of the primary key columns.</param>
    /// ###
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, DataTable dt, DataRowState state, IEnumerable<string> primaryKeyColumnNames)
    {
        if (dt.Rows.Count > 0)
        {
            var bulkOperation = new SqlBulkOperation
                {
                    SqlBulkCopy = obj,
                    DataSource = dt,
                    DataRowState = state,
                    PrimaryKeys = primaryKeyColumnNames.ToList()
                };
            bulkOperation.BulkUpdate();
        }
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="reader">The reader.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, IDataReader reader)
    {
        var bulkOperation = new SqlBulkOperation
            {
                SqlBulkCopy = obj,
                DataSource = reader
            };
        bulkOperation.BulkUpdate();
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="reader">The reader.</param>
    /// <param name="primaryKeyColumnNames">List of names of the primary key columns.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate(this SqlBulkCopy obj, IDataReader reader, IEnumerable<string> primaryKeyColumnNames)
    {
        var bulkOperation = new SqlBulkOperation
            {
                SqlBulkCopy = obj,
                DataSource = reader,
                PrimaryKeys = primaryKeyColumnNames.ToList()
            };
        bulkOperation.BulkUpdate();
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="entities">The entities.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate<T>(this SqlBulkCopy obj, IEnumerable<T> entities)
    {
        var bulkOperation = new SqlBulkOperation
            {
                SqlBulkCopy = obj,
                DataSource = entities,
            };
        bulkOperation.BulkUpdate();
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk update.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="entities">The entities.</param>
    /// <param name="primaryKeyColumnNames">List of names of the primary key columns.</param>
    /// <example>
    ///     <code>
    ///           using System.Collections.Generic;
    ///           using System.Data;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkUpdate()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataTableTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataTableTest, new[] {&quot;ID1&quot;, &quot;ID2&quot;});
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 61;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 125;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkUpdate(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.BatchSize = 77;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.ColumnMappings.Add(&quot;ID1&quot;, &quot;ID1&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ID2&quot;, &quot;ID2&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueBit&quot;, &quot;ValueBit&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueInt&quot;, &quot;ValueInt&quot;);
    ///                               copy.ColumnMappings.Add(&quot;ValueString&quot;, &quot;ValueString&quot;);
    ///           
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkUpdate(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkUpdate<T>(this SqlBulkCopy obj, IEnumerable<T> entities, IEnumerable<string> primaryKeyColumnNames)
    {
        var bulkOperation = new SqlBulkOperation
            {
                SqlBulkCopy = obj,
                DataSource = entities,
                PrimaryKeys = primaryKeyColumnNames.ToList()
            };
        bulkOperation.BulkUpdate();
    }
}