// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.SqlClient;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Executes the bulk action 2 operation.
        /// </summary>
        public void ExecuteBulkAction2()
        {
            using (var sqlCommand = new SqlCommand(SqlBulkAction2, SqlConnection))
            {
                sqlCommand.Transaction = SqlTransaction;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}