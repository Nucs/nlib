// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Check if the bulk operation is valid.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <returns>true if valid bulk operation, false if not.</returns>
        public bool IsValidBulkOperation()
        {
            if (PrimaryKeys.Count == 0)
            {
                throw new Exception("Primary keys column name cannot be empty, at least only column must be used as primary key.");
            }
            if (SqlBulkCopy.ColumnMappings.Count == 0)
            {
                throw new Exception("Column mapping must be specified for Bulk operation.");
            }
            if (PrimaryKeys.Any(x => !MappingList.Exists(y => y.DestinationColumn == x)))
            {
                throw new Exception("All primary key column name must be mapped.");
            }
            if (!(DataSource is DataTable || DataSource is DataRow[] || DataSource is IDataReader || DataSource is IEnumerable<object>))
            {
                throw new Exception("Invalid datasource");
            }

            return true;
        }
    }
}