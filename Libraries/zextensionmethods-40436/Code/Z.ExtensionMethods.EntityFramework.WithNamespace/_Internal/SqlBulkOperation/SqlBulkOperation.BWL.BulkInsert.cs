// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Bulk insert.
        /// </summary>
        public void BulkInsert()
        {
            SetCommonSetting();
            SetInternalDataSource();
            WriteToServer(false);
        }
    }
}