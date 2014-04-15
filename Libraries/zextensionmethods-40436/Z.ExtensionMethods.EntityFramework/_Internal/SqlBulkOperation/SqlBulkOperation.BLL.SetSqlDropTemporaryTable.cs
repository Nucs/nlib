// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Sets SQL drop temporary table.
        /// </summary>
        public void SetSqlDropTemporaryTable()
        {
            SqlDropTableAction = string.Format(SqlDropTable, TemporaryTableName);
        }
    }
}