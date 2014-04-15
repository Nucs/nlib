using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Z.ExtensionMethods.EF6.Examples
{
    public class System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst
    {
        public static void Insert(int nbItem)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID2");
            dt.Columns.Add("ValueBit");
            dt.Columns.Add("ValueInt");
            dt.Columns.Add("ValueString");

            for (int i = 0; i < nbItem; i++)
            {
                dt.Rows.Add(i, Convert.ToBoolean(1%2), i, "Value" + i);
            }

            using (var conn = new SqlConnection(My.Config.ConnectionString.CodeFirstConnectionString.ConnectionString))
            {
                using (var copy = new SqlBulkCopy(conn))
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    copy.DestinationTableName = "BulkCopyTest";
                    conn.Open();
                    copy.BulkInsert(dt);
                }
            }
        }

        public static List<BulkCopyTestCodeFirst> InsertToEntities(int nbItem)
        {
            var list = new List<BulkCopyTestCodeFirst>();
            for (int i = 0; i < nbItem; i++)
            {
                list.Add(new BulkCopyTestCodeFirst
                    {
                        ID2 = i,
                        ValueBit = Convert.ToBoolean(1%2),
                        ValueInt = i,
                        ValueString = "Value" + i
                    });
            }

            return list;
        }

        public static IEnumerable<BulkCopyTestCodeFirst> InsertToEntities(IEnumerable<BulkCopyTestCodeFirst> entities, int nbItem)
        {
            List<BulkCopyTestCodeFirst> list = entities.ToList();
            for (int i = 0; i < nbItem; i++)
            {
                list.Add(new BulkCopyTestCodeFirst
                    {
                        ID1 = -1,
                        ID2 = i,
                        ValueBit = Convert.ToBoolean(1%2),
                        ValueInt = i,
                        ValueString = "Value" + i
                    });
            }

            return list;
        }

        public static int ItemCount()
        {
            using (var conn = new SqlConnection(My.Config.ConnectionString.CodeFirstConnectionString.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT COUNT(1) FROM BulkCopyTest", conn))
                {
                    conn.Open();
                    return command.ExecuteScalarAs<int>();
                }
            }
        }

        public static void InitializeDB()
        {
            using (var ctx = new CodeFirstContext())
            {
                List<BulkCopyTestCodeFirst> bulkItem = ctx.BulkCopyTestCodeFirsts.ToList();
            }
        }

        public static void CleanData()
        {
            using (var conn = new SqlConnection(My.Config.ConnectionString.CodeFirstConnectionString.ConnectionString))
            {
                using (var command = new SqlCommand("TRUNCATE TABLE BulkCopyTest", conn))
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int IntColumnSum()
        {
            using (var conn = new SqlConnection(My.Config.ConnectionString.CodeFirstConnectionString.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT SUM(ValueInt) FROM BulkCopyTest", conn))
                {
                    conn.Open();
                    return command.ExecuteScalarAs<int>();
                }
            }
        }

        public static DataTable SelectValue()
        {
            using (var conn = new SqlConnection(My.Config.ConnectionString.CodeFirstConnectionString.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT ID1, ID2, ValueBit, ValueInt, ValueString FROM BulkCopyTest", conn))
                {
                    conn.Open();
                    return command.ExecuteDataTable();
                }
            }
        }

        public static DataTable SelectValueMultiplyBy2()
        {
            using (var conn = new SqlConnection(My.Config.ConnectionString.CodeFirstConnectionString.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT ID1, ID2, ValueBit, ValueInt * 2 AS ValueInt, ValueString FROM BulkCopyTest", conn))
                {
                    conn.Open();
                    return command.ExecuteDataTable();
                }
            }
        }
    }
}