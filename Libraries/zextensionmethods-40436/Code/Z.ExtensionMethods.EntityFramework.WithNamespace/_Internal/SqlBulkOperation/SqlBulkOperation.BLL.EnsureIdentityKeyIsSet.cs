// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.SqlClient;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Ensures that identity key is set if not specified.
        /// </summary>
        public void EnsureIdentityKeyIsSet()
        {
            if (string.IsNullOrEmpty(IdentityColumn))
            {
                using (var sqlCommand = new SqlCommand(SqlIdentityColumn, SqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ObjectName", SqlBulkCopy.DestinationTableName);
                    sqlCommand.Transaction = SqlTransaction;
                    object identity = sqlCommand.ExecuteScalar();
                    if (identity != null)
                    {
                        IdentityColumn = identity.ToString();
                    }
                }
            }
        }
    }
}