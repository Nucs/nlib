using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_SqlClient_SqlBulkCopy_BulkInsert
    {
        [TestMethod]
        public void BulkInsert()
        {
            // Delete from DataTable
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            DataTable dtDataTableTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

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
                    copy.BulkInsert(dtDataTableTest);

                    // Unit Test
                    Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                }
            }

            // Delete from DataRow[]
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            DataTable dtDataRowTest = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectValue();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    foreach (DataColumn dc in dtDataRowTest.Columns)
                    {
                        copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    copy.BatchSize = 2;
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkInsert(dtDataRowTest);

                    // Unit Test
                    Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                }
            }


            // Delete from IDataReader
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

            using (var connReader = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest", connReader))
                {
                    connReader.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
                        Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

                        using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
                        {
                            using (var copy = new SqlBulkCopy(conn))
                            {
                                foreach (DataColumn dc in dtDataRowTest.Columns)
                                {
                                    copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                                }
                                copy.BatchSize = 25;
                                copy.DestinationTableName = "dbo.BulkCopyTest";
                                conn.Open();

                                // Examples
                                copy.BulkInsert(reader);

                                // Unit Test
                                Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                            }
                        }
                    }
                }
            }

            // Delete from Entities
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            IEnumerable<System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity> entities = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    copy.ColumnMappings.Add("ID1", "ID1");
                    copy.ColumnMappings.Add("ID2", "ID2");
                    copy.ColumnMappings.Add("ValueBit", "ValueBit");
                    copy.ColumnMappings.Add("ValueInt", "ValueInt");
                    copy.ColumnMappings.Add("ValueString", "ValueString");

                    copy.BatchSize = 74;
                    copy.DestinationTableName = "dbo.BulkCopyTest";
                    conn.Open();

                    // Examples
                    copy.BulkInsert(entities);

                    // Unit Test
                    Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                }
            }

            // Delete from Entities Array
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            IEnumerable<System_Data_SqlClient_SqlBulkCopy_TestHelper.TestEntity> entitiesArray = System_Data_SqlClient_SqlBulkCopy_TestHelper.SelectEntities();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

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
                    copy.BulkInsert(entitiesArray.ToArray());

                    // Unit Test
                    Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                }
            }
        }
    }
}