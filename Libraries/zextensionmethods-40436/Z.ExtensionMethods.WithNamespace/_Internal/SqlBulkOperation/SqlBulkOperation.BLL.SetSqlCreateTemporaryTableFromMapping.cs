// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Linq;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Sets SQL create temporary table from mapping.
        /// </summary>
        public void SetSqlCreateTemporaryTableFromMapping()
        {
            string selectColumn = string.Join(", ", MappingList.Select(x => string.Concat("[", x.DestinationColumn, "]")));
            if (IdentityColumn != "")
            {
                string oldValue = string.Concat("[", IdentityColumn, "]");
                string newValue = string.Concat("[", IdentityColumn, "] AS [", IdentityColumn, "]");
                selectColumn = selectColumn.Replace(oldValue, newValue);
            }
            if (SqlBulkCopy.BatchSize > 0)
            {
                string oldValue = string.Concat("[", TemporaryColumnName, "]");
                string newValue = string.Concat("0 AS [", TemporaryColumnName, "]");
                selectColumn = selectColumn.Replace(oldValue, newValue);
            }
            SqlCreateTemporaryTableAction = string.Format(SqlSelectInto, SqlBulkCopy.DestinationTableName, TemporaryTableName, selectColumn);
        }
    }
}