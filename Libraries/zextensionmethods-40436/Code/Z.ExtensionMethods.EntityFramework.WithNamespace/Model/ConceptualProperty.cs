// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.EntityFramework.Model
{
    public class ConceptualProperty
    {
        /// <summary>The complex entity.</summary>
        public ConceptualEntity ComplexEntity;

        /// <summary>true if this object is fixed length.</summary>
        public bool IsFixedLength;

        /// <summary>true if this object is maximum length.</summary>
        public bool IsMaxLength;

        /// <summary>true if this object is nullable.</summary>
        public bool IsNullable;

        /// <summary>true if this object is primary key.</summary>
        public bool IsPrimaryKey;

        /// <summary>true if this object is unicode.</summary>
        public bool IsUnicode;

        /// <summary>The mapping.</summary>
        public StorageColumn Mapping;

        /// <summary>The maximum length.</summary>
        public int? MaxLength;

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>The parent entity.</summary>
        public ConceptualEntity ParentEntity;

        /// <summary>The precision.</summary>
        public int? Precision;

        /// <summary>The scale.</summary>
        public int? Scale;

        /// <summary>The type.</summary>
        public string Type;
    }
}