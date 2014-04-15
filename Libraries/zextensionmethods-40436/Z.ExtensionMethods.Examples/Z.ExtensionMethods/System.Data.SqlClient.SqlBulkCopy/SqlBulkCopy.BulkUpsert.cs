using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_SqlClient_SqlBulkCopy_BulkUpsert
    {
        [TestMethod]
        public void BulkUpsert()
        {
            // Delete from DataTable
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.InsertToDataTable(dtDataTableTest, 100);


            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    foreach (DataColumn dc in dtDataTableTest.Columns)
                    {
                        copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkUpsert(dtDataTableTest, new[] {"ID1", "ID2"});

                    // Unit Test
                    Assert.AreEqual(200, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                    Assert.AreEqual(14850, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }

            // Delete from DataRow[]
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.InsertToDataTable(dtDataRowTest, 100);

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    foreach (DataColumn dc in dtDataRowTest.Columns)
                    {
                        copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    copy.BatchSize = 37;
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkUpsert(dtDataRowTest);

                    // Unit Test
                    Assert.AreEqual(200, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                    Assert.AreEqual(14850, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }


            // Delete from IDataReader
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());

            using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest", connReader))
                {
                    connReader.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
                        {
                            using (var copy = new SqlBulkCopy(conn))
                            {
                                foreach (DataColumn dc in dtDataRowTest.Columns)
                                {
                                    copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                                }
                                copy.BatchSize = 74;
                                copy.DestinationTableName = "dbo.BulkCopyTest";
                                conn.Open();

                                // Examples
                                copy.BulkUpsert(reader);

                                // Unit Test
                                Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                                Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                            }
                        }
                    }
                }
            }

            // Delete from Entities
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            IEnumerable<System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity> entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
            entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.InsertToEntities(entities, 100);

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    copy.ColumnMappings.Add("ID1", "ID1");
                    copy.ColumnMappings.Add("ID2", "ID2");
                    copy.ColumnMappings.Add("ValueBit", "ValueBit");
                    copy.ColumnMappings.Add("ValueInt", "ValueInt");
                    copy.ColumnMappings.Add("ValueString", "ValueString");

                    copy.BatchSize = 22;
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkUpsert(entities);

                    // Unit Test
                    Assert.AreEqual(200, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                    Assert.AreEqual(14850, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }

            // Delete from Entities Array
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            IEnumerable<System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity> entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();
            entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.InsertToEntities(entitiesArray, 100);

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    copy.ColumnMappings.Add("ID1", "ID1");
                    copy.ColumnMappings.Add("ID2", "ID2");
                    copy.ColumnMappings.Add("ValueBit", "ValueBit");
                    copy.ColumnMappings.Add("ValueInt", "ValueInt");
                    copy.ColumnMappings.Add("ValueString", "ValueString");

                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkUpsert(entitiesArray.ToArray());

                    // Unit Test
                    Assert.AreEqual(200, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                    Assert.AreEqual(14850, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }
        }
    }
}