// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Bulk delete.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        public void BulkDelete()
        {
            SetCommonSetting();
            EnsurePrimaryKeyIsSet();
            EnsureIdentityKeyIsSet();

            if (IsValidBulkOperation())
            {
                SetInternalDataSource();
                AddCustomMapping();

                // Set SQL
                SetSqlCreateTemporaryTableFromMapping();
                SetSqlBulkDeleteAction();
                SetSqlDropTemporaryTable();

                try
                {
                    // Execute Bulk Operation
                    EnsureTransaction();
                    CreateTemporaryTable();
                    WriteToServer();
                    ExecuteBulkAction();
                    DeleteTemporaryTable();
                    RemoveCustomMapping();
                    if (IsCustomTransaction)
                    {
                        SqlTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    if (IsCustomTransaction)
                    {
                        SqlTransaction.Rollback();
                    }
                    throw;
                }
            }
        }
    }
}