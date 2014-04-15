// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data.SqlClient;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Ensures that primary key is set if not specified.
        /// </summary>
        public void EnsurePrimaryKeyIsSet()
        {
            if (PrimaryKeys == null || PrimaryKeys.Count == 0)
            {
                PrimaryKeys = new List<string>();
                using (var sqlCommand = new SqlCommand(SqlPrimaryKey, SqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ObjectName", SqlBulkCopy.DestinationTableName);
                    sqlCommand.Transaction = SqlTransaction;
                    using (SqlDataReader dr = sqlCommand.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PrimaryKeys.Add(dr[0].ToString());
                        }
                    }
                }
            }
        }
    }
}