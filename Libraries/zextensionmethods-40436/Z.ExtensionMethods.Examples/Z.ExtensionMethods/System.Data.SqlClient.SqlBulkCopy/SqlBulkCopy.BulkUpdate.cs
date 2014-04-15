using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_SqlClient_SqlBulkCopy_BulkUpdate
    {
        [TestMethod]
        public void BulkUpdate()
        {
            // Delete from DataTable
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();

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
                    copy.BulkUpdate(dtDataTableTest, new[] {"ID1", "ID2"});

                    // Unit Test
                    Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }

            // Delete from DataRow[]
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValueMultiplyBy2();

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    foreach (DataColumn dc in dtDataRowTest.Columns)
                    {
                        copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    copy.BatchSize = 61;
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkUpdate(dtDataRowTest);

                    // Unit Test
                    Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }


            // Delete from IDataReader
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
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
                                copy.BatchSize = 125;
                                copy.DestinationTableName = "dbo.BulkCopyTest";
                                conn.Open();

                                // Examples
                                copy.BulkUpdate(reader);

                                // Unit Test
                                Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                            }
                        }
                    }
                }
            }

            // Delete from Entities
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            IEnumerable<System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity> entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    copy.ColumnMappings.Add("ID1", "ID1");
                    copy.ColumnMappings.Add("ID2", "ID2");
                    copy.ColumnMappings.Add("ValueBit", "ValueBit");
                    copy.ColumnMappings.Add("ValueInt", "ValueInt");
                    copy.ColumnMappings.Add("ValueString", "ValueString");

                    copy.BatchSize = 77;
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkUpdate(entities);

                    // Unit Test
                    Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }

            // Delete from Entities Array
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
            IEnumerable<System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity> entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntitiesMultiplyBy2();

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
                    copy.BulkUpdate(entitiesArray.ToArray());

                    // Unit Test
                    Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }
        }
    }
}