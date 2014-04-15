// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Linq;

namespace Z.EntityFramework.Model
{
    public class StorageColumn
    {
        /// <summary>true if this object is computed.</summary>
        public bool IsComputed;

        /// <summary>true if this object is fixed length.</summary>
        public bool IsFixedLength;

        /// <summary>true if this object is identity.</summary>
        public bool IsIdentity;

        /// <summary>true if this object is nullable.</summary>
        public bool IsNullable;

        /// <summary>true if this object is primary key.</summary>
        public bool IsPrimaryKey;

        /// <summary>The mappings.</summary>
        public List<ConceptualProperty> Mappings;

        /// <summary>The maximum length.</summary>
        public int? MaxLength;

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>The parent table.</summary>
        public StorageTable ParentTable;

        /// <summary>The precision.</summary>
        public int? Precision;

        /// <summary>The scale.</summary>
        public int? Scale;

        /// <summary>The type.</summary>
        public string Type;

        /// <summary>Gets the mapping.</summary>
        /// <value>The mapping.</value>
        public ConceptualProperty Mapping
        {
            get { return Mappings == null || Mappings.Count == 0 ? null : Mappings.First(); }
        }
    }
}