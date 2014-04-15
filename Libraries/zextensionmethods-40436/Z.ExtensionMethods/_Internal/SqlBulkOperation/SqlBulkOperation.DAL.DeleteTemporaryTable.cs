// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data.SqlClient;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Deletes the temporary table.
        /// </summary>
        public void DeleteTemporaryTable()
        {
            using (var sqlCommand = new SqlCommand(String.Format(SqlDropTableAction, TemporaryTableName), SqlConnection))
            {
                sqlCommand.Transaction = SqlTransaction;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}