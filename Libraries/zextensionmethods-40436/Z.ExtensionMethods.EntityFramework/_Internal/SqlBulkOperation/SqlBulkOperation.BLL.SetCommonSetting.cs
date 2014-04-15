// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Sets common setting.
        /// </summary>
        public void SetCommonSetting()
        {
            Type type = SqlBulkCopy.GetType();
            SqlConnection = type.GetField("_connection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(SqlBulkCopy) as SqlConnection;
            SqlTransaction = type.GetField("_externalTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(SqlBulkCopy) as SqlTransaction;
            TemporaryTableName = string.Concat("#", Guid.NewGuid().ToString().Replace("-", "_"));
            TemporaryColumnName = Guid.NewGuid().ToString().Replace("-", "_");
        }
    }
}