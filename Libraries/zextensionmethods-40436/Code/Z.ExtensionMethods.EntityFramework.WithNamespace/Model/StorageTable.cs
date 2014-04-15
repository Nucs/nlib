// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Linq;

namespace Z.EntityFramework.Model
{
    public class StorageTable
    {
        /// <summary>The columns.</summary>
        public List<StorageColumn> Columns;

        /// <summary>Name of the entity.</summary>
        public string EntityName;

        /// <summary>The mappings.</summary>
        public List<ConceptualEntity> Mappings = new List<ConceptualEntity>();

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>The primary key.</summary>
        public StoragePrimaryKey PrimaryKey;

        /// <summary>The schema.</summary>
        public string Schema;

        /// <summary>The type.</summary>
        public string Type = "";

        /// <summary>Gets the mapping.</summary>
        /// <value>The mapping.</value>
        public ConceptualEntity Mapping
        {
            get { return Mappings == null || Mappings.Count == 0 ? null : Mappings.First(); }
        }
    }
}