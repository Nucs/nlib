// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;
using System.Data.SqlClient;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Gets table column.
        /// </summary>
        /// <returns>The table column.</returns>
        public DataTable GetTableColumn()
        {
            string sqlSelectColumn = string.Format(SqlGetTableColumn, SqlBulkCopy.DestinationTableName);
            using (var sqlCommand = new SqlCommand(sqlSelectColumn, SqlConnection))
            {
                var ds = new DataSet();
                using (var sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(ds);
                }

                return ds.Tables[0];
            }
        }
    }
}