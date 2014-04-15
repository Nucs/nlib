// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.EntityFramework.Model
{
    public class StorageModel
    {
        /// <summary>The functions.</summary>
        public List<StorageFunction> Functions;

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>The tables.</summary>
        public List<StorageTable> Tables;
    }
}