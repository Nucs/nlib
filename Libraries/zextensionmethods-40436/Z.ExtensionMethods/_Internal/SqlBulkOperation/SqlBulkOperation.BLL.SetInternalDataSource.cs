// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Sets internal data source.
        /// </summary>
        public void SetInternalDataSource()
        {
            if (SqlBulkCopy.BatchSize == 0)
            {
                if (DataSource is DataTable || DataSource is DataRow[] || DataSource is IDataReader)
                {
                    InternalDataSource = DataSource;
                }
                else if (DataSource is IEnumerable<object>)
                {
                    SetInternalDataSourceObjectMapping();
                }
            }
            else
            {
                if (DataSource is DataTable)
                {
                    // Create a DataTable copy.
                    InternalDataSource = (DataSource as DataTable).Copy();
                }
                else if (DataSource is DataRow[])
                {
                    // Import all row in a new DataTable.
                    var dt = new DataTable();
                    foreach (DataRow dr in (DataRow[]) DataSource)
                    {
                        dt.ImportRow(dr);
                    }

                    InternalDataSource = dt;
                }
                else if (DataSource is IDataReader)
                {
                    // Load DataReader in a new DataTable.
                    var dt = new DataTable();
                    dt.Load(DataSource as IDataReader);

                    InternalDataSource = dt;
                }
                else if (DataSource is IEnumerable<object>)
                {
                    // Create a DataTable from the IEnumerable<object>.
                    SetInternalDataSourceObjectMapping();
                }

                // Add a custom "identity" column.
                var dtSource = (DataTable) InternalDataSource;
                dtSource.Columns.Add(TemporaryColumnName);
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    dtSource.Rows[i][TemporaryColumnName] = i + 1;
                }

                InternalDataSource = dtSource;
            }
        }
    }
}