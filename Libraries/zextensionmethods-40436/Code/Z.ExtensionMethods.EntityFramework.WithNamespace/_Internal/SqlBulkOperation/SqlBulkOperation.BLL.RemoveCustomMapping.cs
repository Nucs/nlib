// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Removes custom temporary column if batch size is specified.
        /// </summary>
        public void RemoveCustomMapping()
        {
            if (SqlBulkCopy.BatchSize > 0)
            {
                SqlBulkCopy.ColumnMappings.RemoveAt(SqlBulkCopy.ColumnMappings.Count - 1);
            }
        }
    }
}