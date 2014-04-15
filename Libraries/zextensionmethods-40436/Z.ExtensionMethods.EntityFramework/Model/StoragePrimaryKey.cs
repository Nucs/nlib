// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.EntityFramework.Model
{
    public class StoragePrimaryKey
    {
        /// <summary>The columns.</summary>
        public List<StorageColumn> Columns;

        /// <summary>The parent table.</summary>
        public StorageTable ParentTable;
    }
}