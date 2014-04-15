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
        ///     Sets SQL bulk update action.
        /// </summary>
        public void SetSqlBulkUpdateAction()
        {
            string sqlSetValue = string.Join(", ", MappingList.Where(x => !PrimaryKeys.Contains(x.DestinationColumn) && x.DestinationColumn != IdentityColumn && x.DestinationColumn != TemporaryColumnName)
                                                              .Select(x => string.Format("A.[{0}] = B.[{0}]", x.DestinationColumn)));
            string sqlWhere = String.Join(" AND ", PrimaryKeys.Select(x => string.Format("A.[{0}]" + "=" + "B.[{0}]", x)));
            string sqlAction = string.Format(SqlActionUpdate, SqlBulkCopy.DestinationTableName, TemporaryTableName, sqlSetValue, sqlWhere);

            SqlBulkAction = SqlBulkCopy.BatchSize == 0
                                ? sqlAction
                                : string.Format(SqlBatchOperation, TemporaryTableName, sqlAction, TemporaryColumnName, SqlBulkCopy.BatchSize);
        }
    }
}