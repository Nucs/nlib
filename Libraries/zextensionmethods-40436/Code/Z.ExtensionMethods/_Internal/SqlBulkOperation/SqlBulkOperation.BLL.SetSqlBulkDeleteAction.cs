// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Linq;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Sets SQL bulk delete action.
        /// </summary>
        public void SetSqlBulkDeleteAction()
        {
            string sqlWhere = String.Join(" AND ", PrimaryKeys.Select(x => string.Format("A.[{0}]" + "=" + "B.[{0}]", x)));

            string sqlAction = string.Format(SqlActionDelete, SqlBulkCopy.DestinationTableName, TemporaryTableName, sqlWhere);
            SqlBulkAction = SqlBulkCopy.BatchSize == 0
                                ? sqlAction
                                : string.Format(SqlBatchOperation, TemporaryTableName, sqlAction, TemporaryColumnName, SqlBulkCopy.BatchSize);
        }
    }
}