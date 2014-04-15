// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Reflection;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Ensures a transaction is used if none is specified.
        /// </summary>
        public void EnsureTransaction()
        {
            if (SqlTransaction == null)
            {
                SqlTransaction = SqlConnection.BeginTransaction();
                SqlBulkCopy.GetType().GetField("_externalTransaction", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(SqlBulkCopy, SqlTransaction);
                IsCustomTransaction = true;
            }
        }
    }
}