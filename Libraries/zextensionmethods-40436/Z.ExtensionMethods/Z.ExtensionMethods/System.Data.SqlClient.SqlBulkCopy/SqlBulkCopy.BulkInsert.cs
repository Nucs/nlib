// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Z.Utility;

public static partial class SqlBulkCopyExtension
{
    /// <summary>
    ///     A SqlBulkCopy extension method that bulk insert.
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
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkInsert
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkInsert()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(dtDataTableTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 2;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                                   Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 25;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkInsert(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BatchSize = 74;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkInsert(this SqlBulkCopy obj, DataRow[] drs)
    {
        obj.WriteToServer(drs);
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk insert.
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
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkInsert
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkInsert()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(dtDataTableTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 2;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                                   Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 25;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkInsert(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BatchSize = 74;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkInsert(this SqlBulkCopy obj, DataTable dt)
    {
        obj.WriteToServer(dt);
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk insert.
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
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkInsert
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkInsert()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(dtDataTableTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 2;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                                   Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 25;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkInsert(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BatchSize = 74;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkInsert(this SqlBulkCopy obj, DataTable dt, DataRowState state)
    {
        obj.WriteToServer(dt, state);
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk insert.
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
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkInsert
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkInsert()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(dtDataTableTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 2;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                                   Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 25;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkInsert(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BatchSize = 74;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkInsert(this SqlBulkCopy obj, IDataReader reader)
    {
        obj.WriteToServer(reader);
    }

    /// <summary>
    ///     A SqlBulkCopy extension method that bulk insert.
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
    ///               public class System_Data_SqlClient_SqlBulkCopy_BulkInsert
    ///               {
    ///                   [TestMethod]
    ///                   public void BulkInsert()
    ///                   {
    ///                       // Delete from DataTable
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(dtDataTableTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from DataRow[]
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                               {
    ///                                   copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                               }
    ///                               copy.BatchSize = 2;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(dtDataRowTest);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///           
    ///                       // Delete from IDataReader
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                       using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           using (var command = new SqlCommand(&quot;SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest&quot;, connReader))
    ///                           {
    ///                               connReader.Open();
    ///                               using (IDataReader reader = command.ExecuteReader())
    ///                               {
    ///                                   System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                                   Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///           
    ///                                   using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                                   {
    ///                                       using (var copy = new SqlBulkCopy(conn))
    ///                                       {
    ///                                           foreach (DataColumn dc in dtDataRowTest.Columns)
    ///                                           {
    ///                                               copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
    ///                                           }
    ///                                           copy.BatchSize = 25;
    ///                                           copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                                           conn.Open();
    ///           
    ///                                           // Examples
    ///                                           copy.BulkInsert(reader);
    ///           
    ///                                           // Unit Test
    ///                                           Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                                       }
    ///                                   }
    ///                               }
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BatchSize = 74;
    ///                               copy.DestinationTableName = &quot;dbo.BulkCopyTest&quot;;
    ///                               conn.Open();
    ///           
    ///                               // Examples
    ///                               copy.BulkInsert(entities);
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///           
    ///                       // Delete from Entities Array
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
    ///                       Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                       IEnumerable&lt;System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity&gt; entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
    ///                       System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
    ///                       Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
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
    ///                               copy.BulkInsert(entitiesArray.ToArray());
    ///           
    ///                               // Unit Test
    ///                               Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
    ///                           }
    ///                       }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static void BulkInsert<T>(this SqlBulkCopy obj, IEnumerable<T> entities)
    {
        var bulkOperation = new SqlBulkOperation
            {
                SqlBulkCopy = obj,
                DataSource = entities
            };
        bulkOperation.BulkInsert();
    }
}