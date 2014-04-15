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
        ///     Sets SQL bulk merge action.
        /// </summary>
        public void SetSqlBulkMergeAction()
        {
            string sqlSetValue = string.Join(", ", MappingList.Where(x => !PrimaryKeys.Contains(x.DestinationColumn) && x.DestinationColumn != IdentityColumn && x.DestinationColumn != TemporaryColumnName)
                                                              .Select(x => string.Format("A.[{0}] = B.[{0}]", x.DestinationColumn)));
            string sqlWhere = String.Join(" AND ", PrimaryKeys.Select(x => string.Format("A.[{0}]" + "=" + "B.[{0}]", x)));
            string sqlActionUpdate = string.Format(SqlActionUpdate, SqlBulkCopy.DestinationTableName, TemporaryTableName, sqlSetValue, sqlWhere);
            string sqlInserColumns = string.Join(", ", MappingList
                                                           .Where(x => x.DestinationColumn != IdentityColumn && x.DestinationColumn != TemporaryColumnName)
                                                           .Select(x => string.Format("[{0}]", x.DestinationColumn)));
            string sqlInsertValue = string.Join(", ", MappingList
                                                          .Where(x => x.DestinationColumn != IdentityColumn && x.DestinationColumn != TemporaryColumnName)
                                                          .Select(x => string.Format("[{0}]", x.DestinationColumn)));
            string sqlActionInsert = string.Format(SqlActionInsert, SqlBulkCopy.DestinationTableName, TemporaryTableName, sqlWhere, sqlInserColumns, sqlInsertValue);

            SqlBulkAction = SqlBulkCopy.BatchSize == 0
                                ? sqlActionUpdate
                                : string.Format(SqlBatchOperation, TemporaryTableName, sqlActionUpdate, TemporaryColumnName, SqlBulkCopy.BatchSize);
            SqlBulkAction2 = SqlBulkCopy.BatchSize == 0
                                 ? sqlActionInsert
                                 : string.Format(SqlBatchOperationAnd, TemporaryTableName, sqlActionInsert, TemporaryColumnName, SqlBulkCopy.BatchSize);
        }
    }
}