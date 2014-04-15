// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Z.ExtensionMethods.ModelMapping
{
    public class Association
    {
        /// <summary>The association ends.</summary>
        [XmlElement("End")] public List<AssociationEnd> AssociationEnds;

        /// <summary>The association referential constraints.</summary>
        [XmlElement("ReferentialConstraint")] public List<AssociationReferentialConstraint> AssociationReferentialConstraints;

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;
    }

    public class AssociationSet
    {
        #region Order

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;

        #endregion

        #region Order

        /// <summary>The association.</summary>
        [XmlAttribute] public string Association;

        #endregion

        /// <summary>The association set ends.</summary>
        [XmlElement("End")] public List<AssociationSetEnd> AssociationSetEnds;
    }

    public class AssociationSetEnd
    {
        #region Order

        /// <summary>The role.</summary>
        [XmlAttribute] public string Role;

        #endregion

        #region Order

        /// <summary>Set the entity belongs to.</summary>
        [XmlAttribute] public string EntitySet;

        #endregion
    }

    public class AssociationEnd
    {
        #region Order

        /// <summary>The role.</summary>
        [XmlAttribute] public string Role;

        #endregion

        #region Order

        /// <summary>The type.</summary>
        [XmlAttribute] public string Type;

        #endregion

        #region Order

        /// <summary>The multiplicity.</summary>
        [XmlAttribute] public string Multiplicity;

        #endregion
    }

    public class AssociationReferentialConstraint
    {
        /// <summary>The dependent.</summary>
        [XmlElement("Dependent", Order = 2)] public List<AssociationReferentialConstraintDependent> Dependent;

        /// <summary>The principals.</summary>
        [XmlElement("Principal", Order = 1)] public List<AssociationReferentialConstraintPrincipal> Principals;
    }

    public class AssociationReferentialConstraintPrincipal
    {
        /// <summary>The properties.</summary>
        [XmlElement("PropertyRef")] public List<Property> Properties;

        /// <summary>The role.</summary>
        [XmlAttribute] public string Role;
    }


    public class AssociationReferentialConstraintDependent
    {
        /// <summary>The properties.</summary>
        [XmlElement("PropertyRef")] public List<Property> Properties;

        /// <summary>The role.</summary>
        [XmlAttribute] public string Role;
    }

    public class ComplexProperty
    {
        #region Order

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;

        #endregion

        #region Order

        /// <summary>Name of the type.</summary>
        [XmlAttribute] public string TypeName;

        #endregion

        /// <summary>The complex properties.</summary>
        [XmlElement("ComplexProperty", Order = 2)] public List<ComplexProperty> ComplexProperties;

        /// <summary>The scalar properties.</summary>
        [XmlElement("ScalarProperty", Order = 1)] public List<ScalarProperty> ScalarProperties;
    }

    public class ConceptualModels
    {
        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2009/11/edm")] public Schema Schema;
    }

    [XmlRoot("Edmx", Namespace = "http://schemas.microsoft.com/ado/2009/11/edmx", IsNullable = false)]
    public class Edmx
    {
        /// <summary>The runtime.</summary>
        public RunTime Runtime;
    }

    public class Condition
    {
        #region Order

        /// <summary>The value.</summary>
        /// <summary>The value.</summary>
        /// <summary>The value.</summary>
        [XmlAttribute] public string Value;

        #endregion

        #region Order

        /// <summary>Name of the column.</summary>
        [XmlAttribute] public string ColumnName;

        #endregion
    }

    public class End
    {
        /// <summary>The multiplicity.</summary>
        [XmlAttribute] public string Multiplicity;

        /// <summary>The role.</summary>
        [XmlAttribute] public string Role;

        /// <summary>The type.</summary>
        [XmlAttribute] public string Type;
    }

    public class EntityContainer
    {
        /// <summary>Sets the association belongs to.</summary>
        [XmlElement("AssociationSet", Order = 1)] public List<AssociationSet> AssociationSets;

        /// <summary>Sets the entity belongs to.</summary>
        [XmlElement("EntitySet", Order = 0)] public List<EntitySet> EntitySets;

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;
    }

    public class EntityContainerMapping
    {
        #region Order

        /// <summary>The storage entity container.</summary>
        [XmlAttribute] public string StorageEntityContainer;

        #endregion

        #region Order

        /// <summary>The cdm entity container.</summary>
        [XmlAttribute] public string CdmEntityContainer;

        #endregion

        /// <summary>The entity set mappings.</summary>
        [XmlElement("EntitySetMapping")] public List<EntitySetMapping> EntitySetMappings;
    }

    public class EntitySet
    {
        #region Order

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;

        #endregion

        #region Order

        /// <summary>Type of the entity.</summary>
        [XmlAttribute] public string EntityType;

        #endregion

        #region Order

        /// <summary>The schema.</summary>
        [XmlAttribute] public string Schema;

        #endregion

        #region Order

        /// <summary>The table.</summary>
        [XmlAttribute] public string Table;

        #endregion
    }

    public class EntitySetMapping
    {
        /// <summary>The entity type mappings.</summary>
        [XmlElement("EntityTypeMapping")] public List<EntityTypeMapping> EntityTypeMappings;

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;
    }

    public class EntityType
    {
        #region Order

        /// <summary>The key.</summary>
        public Key Key;

        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;

        #endregion

        #region Order

        /// <summary>Type of the base.</summary>
        [XmlAttribute] public string BaseType;

        #endregion

        #region Order

        /// <summary>true to abstract.</summary>
        [DefaultValue(false)] [XmlAttribute] public bool Abstract;

        #endregion

        /// <summary>The properties.</summary>
        [XmlElement("Property")] public List<Property> Properties;
    }

    public class EntityTypeMapping
    {
        /// <summary>The mapping fragment.</summary>
        public MappingFragment MappingFragment;

        /// <summary>Name of the type.</summary>
        [XmlAttribute] public string TypeName;
    }

    public class Key
    {
        /// <summary>The property references.</summary>
        [XmlElement("PropertyRef")] public List<Property> PropertyRefs;
    }

    public class Mapping
    {
        /// <summary>The entity container mapping.</summary>
        public EntityContainerMapping EntityContainerMapping;

        /// <summary>The space.</summary>
        [XmlAttribute] public string Space;
    }

    public class MappingFragment
    {
        /// <summary>The complex properties.</summary>
        [XmlElement("ComplexProperty", Order = 2)] public List<ComplexProperty> ComplexProperties;

        /// <summary>The conditions.</summary>
        [XmlElement("Condition", Order = 3)] public List<Condition> Conditions;

        /// <summary>The scalar properties.</summary>
        [XmlElement("ScalarProperty", Order = 1)] public List<ScalarProperty> ScalarProperties;

        /// <summary>Set the store entity belongs to.</summary>
        [XmlAttribute] public string StoreEntitySet;
    }

    public class Mappings
    {
        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2009/11/mapping/cs")] public Mapping Mapping;
    }

    public class Property
    {
        #region Order

        /// <summary>The name.</summary>
        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;

        #endregion

        #region Order

        /// <summary>The type.</summary>
        [XmlAttribute] public string Type;

        #endregion

        #region Order

        /// <summary>A pattern specifying the store generated.</summary>
        [XmlAttribute] public string StoreGeneratedPattern;

        #endregion

        #region Order

        /// <summary>true if nullable.</summary>
        [DefaultValue(true)] [XmlAttribute] public bool Nullable = true;

        #endregion

        #region Order

        /// <summary>The maximum length.</summary>
        [XmlAttribute] public string MaxLength;

        #endregion

        #region Order

        /// <summary>true to fixed length.</summary>
        [DefaultValue(true)] [XmlAttribute] public bool FixedLength = true;

        #endregion

        #region Order

        /// <summary>true to unicode.</summary>
        [DefaultValue(false)] [XmlAttribute] public bool Unicode;

        #endregion

        #region Order

        [XmlAttribute("StoreGeneratedPattern", Namespace = "http://schemas.microsoft.com/ado/2009/02/edm/annotation")] public string AnnotationStoreGeneratedPattern;

        #endregion
    }

    public class RunTime
    {
        /// <summary>The conceptual models.</summary>
        public ConceptualModels ConceptualModels;

        /// <summary>The mappings.</summary>
        public Mappings Mappings;

        /// <summary>The storage models.</summary>
        public StorageModels StorageModels;
    }

    public class ScalarProperty
    {
        #region Order

        /// <summary>The name.</summary>
        /// <summary>The name.</summary>
        [XmlAttribute] public string Name;

        #endregion

        #region Order

        /// <summary>Name of the column.</summary>
        [XmlAttribute] public string ColumnName;

        #endregion
    }

    public class Schema
    {
        /// <summary>The alias.</summary>
        [XmlAttribute] public string Alias;

        /// <summary>The associations.</summary>
        [XmlElement("Association", Order = 3)] public List<Association> Associations;

        /// <summary>List of types of the complexes.</summary>
        [XmlElement("ComplexType", Order = 1)] public List<EntityType> ComplexTypes;

        /// <summary>The entity container.</summary>
        [XmlElement("EntityContainer", Order = 4)] public EntityContainer EntityContainer;

        /// <summary>List of types of the entities.</summary>
        [XmlElement("EntityType", Order = 2)] public List<EntityType> EntityTypes;

        /// <summary>The namespace.</summary>
        [XmlAttribute] public string Namespace;

        /// <summary>The provider.</summary>
        [XmlAttribute] public string Provider;
    }


    public class StorageModels
    {
        /// <summary>.</summary>
        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl")] public Schema Schema;
    }
}