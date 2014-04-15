// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Writes to server.
        /// </summary>
        /// <param name="overrideDestinationTable">(Optional) the override destination table.</param>
        public void WriteToServer(bool overrideDestinationTable = true)
        {
            string originalDestinationName = SqlBulkCopy.DestinationTableName;
            if (overrideDestinationTable && TemporaryTableName != "")
            {
                SqlBulkCopy.DestinationTableName = string.Concat("[", TemporaryTableName + "]");
            }

            if (InternalDataSource is DataTable)
            {
                if (DataRowState.HasValue)
                {
                    SqlBulkCopy.WriteToServer(InternalDataSource as DataTable, DataRowState.Value);
                }
                else
                {
                    SqlBulkCopy.WriteToServer(InternalDataSource as DataTable);
                }
            }
            else if (InternalDataSource is DataRow[])
            {
                SqlBulkCopy.WriteToServer(InternalDataSource as DataRow[]);
            }
            else if (InternalDataSource is IDataReader)
            {
                SqlBulkCopy.WriteToServer(InternalDataSource as IDataReader);
            }

            if (overrideDestinationTable)
            {
                SqlBulkCopy.DestinationTableName = originalDestinationName;
            }
        }
    }
}