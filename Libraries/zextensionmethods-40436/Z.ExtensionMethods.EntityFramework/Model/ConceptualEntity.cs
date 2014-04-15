// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Linq;

namespace Z.EntityFramework.Model
{
    public class ConceptualEntity
    {
        /// <summary>The base entity.</summary>
        public ConceptualEntity BaseEntity;

        /// <summary>Type of the base.</summary>
        public string BaseType;

        /// <summary>Type of the entity.</summary>
        public string EntityType;

        /// <summary>true if this object is complex.</summary>
        public bool IsComplex;

        /// <summary>true if this object is tpc.</summary>
        public bool IsTPC;

        /// <summary>true if this object is tph.</summary>
        public bool IsTPH;

        /// <summary>true if this object is tpt.</summary>
        public bool IsTPT;

        /// <summary>The key.</summary>
        public ConceptualKey Key;

        /// <summary>The mappings.</summary>
        public List<StorageTable> Mappings = new List<StorageTable>();

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>The parent complex entities.</summary>
        public List<ConceptualEntity> ParentComplexEntities;

        /// <summary>The properties.</summary>
        public List<ConceptualProperty> Properties;

        /// <summary>Name of the set.</summary>
        public string SetName;

        /// <summary>The type.</summary>
        public string Type = "";

        /// <summary>Gets the parent complex entity.</summary>
        /// <value>The parent complex entity.</value>
        public ConceptualEntity ParentComplexEntity
        {
            get { return ParentComplexEntities == null || ParentComplexEntities.Count == 0 ? null : ParentComplexEntities.First(); }
        }

        /// <summary>Gets the mapping.</summary>
        /// <value>The mapping.</value>
        public StorageTable Mapping
        {
            get { return Mappings == null || Mappings.Count == 0 ? null : Mappings.First(); }
        }
    }
}