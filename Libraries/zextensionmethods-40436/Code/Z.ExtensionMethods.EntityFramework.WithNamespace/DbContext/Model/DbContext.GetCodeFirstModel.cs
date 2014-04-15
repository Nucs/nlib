// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using System.Xml.Linq;
using Z.EntityFramework.Model;
using Z.ExtensionMethods.ModelMapping;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DbContexttExtension
    {
        /// <summary>
        ///     A DbContext extension method that gets code first model.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The code first model.</returns>
        public static Model GetCodeFirstModel(this DbContext @this)
        {
            MemoryCache cache = MemoryCache.Default;

            if (cache.Contains(@this.ToString()))
            {
                return (Model) cache["GetCodeFirstModel;" + @this];
            }

            Edmx dbmapping = GetMapping(@this);

            var model = new Model();
            BuildStorage(model, dbmapping);
            BuildConceptual(model, dbmapping);
            BuildMapping(model, dbmapping);

            cache.Add(new CacheItem("GetCodeFirstModel;" + @this, model), new CacheItemPolicy());

            return model;
        }

        /// <summary>
        ///     Gets a mapping.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The mapping.</returns>
        internal static Edmx GetMapping(DbContext context)
        {
            XDocument xDoc = context.GetModelXDocument();
            string s = xDoc.ToString();
            var entity = s.DeserializeXml<Edmx>();
            return entity;
        }

        /// <summary>
        ///     Builds a storage.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="edmx">The edmx.</param>
        internal static void BuildStorage(Model model, Edmx edmx)
        {
            StorageModels storage = edmx.Runtime.StorageModels;

            model.Storage = new StorageModel();
            model.Storage.Tables = new List<StorageTable>();

            foreach (EntityType entityType in storage.Schema.EntityTypes)
            {
                var storageTable = new StorageTable
                    {
                        EntityName = entityType.Name,
                        Columns = entityType.Properties.Select(x => new StorageColumn
                            {
                                Name = x.Name,
                                Type = x.Type,
                                IsIdentity = x.StoreGeneratedPattern == "Identity",
                                MaxLength = string.IsNullOrEmpty(x.MaxLength) ? (int?) null : Convert.ToInt32(x.MaxLength),
                                IsNullable = x.Nullable
                            }).ToList()
                    };

                storageTable.Columns.ForEach(x => x.ParentTable = storageTable);
                storageTable.PrimaryKey = new StoragePrimaryKey();
                storageTable.PrimaryKey.Columns = storageTable.Columns.Where(x => entityType.Key.PropertyRefs.Any(y => y.Name == x.Name)).ToList();
                storageTable.PrimaryKey.Columns.ForEach(x => x.IsPrimaryKey = true);

                EntitySet entitySet = storage.Schema.EntityContainer.EntitySets.Find(x => x.Name == storageTable.EntityName);
                storageTable.Schema = entitySet.Schema;
                storageTable.Name = entitySet.Table;

                model.Storage.Tables.Add(storageTable);
            }
        }

        /// <summary>
        ///     Builds a conceptual.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="edmx">The edmx.</param>
        internal static void BuildConceptual(Model model, Edmx edmx)
        {
            ConceptualModels conceptual = edmx.Runtime.ConceptualModels;

            model.Conceptual = new ConceptualModel();
            model.Conceptual.Entities = new List<ConceptualEntity>();

            foreach (EntityType entityType in conceptual.Schema.ComplexTypes.Union(conceptual.Schema.EntityTypes))
            {
                var conceptualEntity = new ConceptualEntity
                    {
                        Name = entityType.Name,
                        BaseType = entityType.BaseType,
                        Properties = entityType.Properties.Select(x => new ConceptualProperty
                            {
                                Name = x.Name,
                                Type = x.Type,
                                IsNullable = x.Nullable,
                                MaxLength = string.IsNullOrEmpty(x.MaxLength) || x.MaxLength == "Max" ? (int?) null : Convert.ToInt32(x.MaxLength),
                                IsFixedLength = x.FixedLength,
                                IsUnicode = x.Unicode
                            }).ToList()
                    };

                conceptualEntity.Properties.ForEach(x => x.ParentEntity = conceptualEntity);
                if (entityType.Key != null)
                {
                    conceptualEntity.Key = new ConceptualKey
                        {
                            Properties = conceptualEntity.Properties.Where(x => entityType.Key != null && entityType.Key.PropertyRefs.Any(y => y.Name == x.Name)).ToList()
                        };
                    conceptualEntity.Key.Properties.ForEach(x => x.IsPrimaryKey = true);
                }


                EntitySet entitySet = conceptual.Schema.EntityContainer.EntitySets.Find(x => x.Name == conceptualEntity.Name);
                if (entitySet != null)
                {
                    conceptualEntity.Type = entitySet.EntityType;
                }

                model.Conceptual.Entities.Add(conceptualEntity);
            }

            // Map Complex Entity
            foreach (ConceptualProperty complex in model.Conceptual.Entities.SelectMany(x => x.Properties.Where(y => y.Type.StartsWith("Self."))))
            {
                string complexEntityName = complex.Type.Substring(5);
                complex.ComplexEntity = model.Conceptual.Entities.Find(x => x.Name == complexEntityName);
                complex.ComplexEntity.IsComplex = true;
            }

            // Map Base
            foreach (ConceptualEntity entity in model.Conceptual.Entities.Where(x => !string.IsNullOrEmpty(x.BaseType) && x.BaseType.StartsWith("Self.")))
            {
                string baseType = entity.BaseType.Substring(5);
                ConceptualEntity baseEntity = model.Conceptual.Entities.Find(x => x.Name == baseType);
                entity.BaseEntity = baseEntity;

                if (baseEntity.ParentComplexEntities == null)
                {
                    baseEntity.ParentComplexEntities = new List<ConceptualEntity>();
                }
                baseEntity.ParentComplexEntities.Add(entity);
            }

            // Map Set Name
            foreach (EntitySet entitySet in conceptual.Schema.EntityContainer.EntitySets)
            {
                string entityName = entitySet.EntityType.Substring(5);
                ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == entityName);
                entity.SetName = entitySet.Name;
            }
        }

        /// <summary>
        ///     Builds a mapping.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="model">The model.</param>
        /// <param name="edmx">The edmx.</param>
        internal static void BuildMapping(Model model, Edmx edmx)
        {
            Mappings mapping = edmx.Runtime.Mappings;

            foreach (EntitySetMapping item in mapping.Mapping.EntityContainerMapping.EntitySetMappings)
            {
                ConceptualEntity conceptualTable = model.Conceptual.Entities.Find(x => x.SetName == item.Name);

                foreach (EntityTypeMapping entityTypeMapping in item.EntityTypeMappings)
                {
                    if (item.EntityTypeMappings.Count > 1)
                    {
                        if (item.EntityTypeMappings.Select(x => x.MappingFragment.StoreEntitySet).Distinct().Count() == 1)
                        {
                            conceptualTable.IsTPH = true;

                            if (!entityTypeMapping.TypeName.StartsWith("IsTypeOf("))
                            {
                                conceptualTable = model.Conceptual.Entities.Find(x => x.Name == entityTypeMapping.TypeName.Split('.').Last());
                            }

                            conceptualTable.IsTPH = true;
                        }
                        else if (!item.EntityTypeMappings.Exists(x => x.TypeName.StartsWith("IsTypeOf(")))
                        {
                            conceptualTable.IsTPC = true;
                            conceptualTable = model.Conceptual.Entities.Find(x => x.Name == entityTypeMapping.TypeName.Split('.').Last());
                            conceptualTable.IsTPC = true;
                        }
                        else
                        {
                            conceptualTable.IsTPT = true;

                            if (!entityTypeMapping.TypeName.StartsWith("IsTypeOf("))
                            {
                                conceptualTable = model.Conceptual.Entities.Find(x => x.Name == entityTypeMapping.TypeName.Split('.').Last());
                            }

                            conceptualTable.IsTPT = true;
                        }
                    }
                    StorageTable storageTable = model.Storage.Tables.Find(x => x.EntityName == entityTypeMapping.MappingFragment.StoreEntitySet);

                    conceptualTable.Mappings.Add(storageTable);
                    storageTable.Mappings.Add(conceptualTable);

                    // Map column
                    foreach (ScalarProperty itemColumn in entityTypeMapping.MappingFragment.ScalarProperties)
                    {
                        ConceptualProperty conceptualColumn = conceptualTable.Properties.Find(x => x.Name == itemColumn.Name);
                        StorageColumn storageColumn = storageTable.Columns.Find(x => x.Name == itemColumn.ColumnName);

                        if (storageColumn == null)
                        {
                            throw new Exception("Storage not found!");
                        }

                        if (conceptualColumn != null)
                        {
                            conceptualColumn.Mapping = storageColumn;
                            if (storageColumn.Mappings == null)
                            {
                                storageColumn.Mappings = new List<ConceptualProperty>();
                            }

                            storageColumn.Mappings.Add(conceptualColumn);
                        }
                        else
                        {
                            List<ConceptualEntity> linkedModel = FindAllInheritEntity(conceptualTable);
                            List<ConceptualProperty> conceptualProperties = linkedModel.SelectMany(y => y.Properties.Where(x => x.Name == itemColumn.Name)).ToList();
                            if (conceptualProperties.Count == 0)
                            {
                                throw new Exception("Property not found!");
                            }
                            conceptualProperties.ForEach(x => x.Mapping = storageColumn);
                            if (storageColumn.Mappings == null)
                            {
                                storageColumn.Mappings = new List<ConceptualProperty>();
                            }
                            storageColumn.Mappings.AddRange(conceptualProperties);
                        }
                    }

                    BuildMappingComplex(model, storageTable, conceptualTable, entityTypeMapping.MappingFragment.ComplexProperties);
                }
            }
        }

        /// <summary>
        ///     Builds mapping complex.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="model">The model.</param>
        /// <param name="storageTable">The storage table.</param>
        /// <param name="conceptualEntity">The conceptual entity.</param>
        /// <param name="complexProperties">The complex properties.</param>
        internal static void BuildMappingComplex(Model model, StorageTable storageTable, ConceptualEntity conceptualEntity, List<ComplexProperty> complexProperties)
        {
            foreach (ComplexProperty complex in complexProperties)
            {
                conceptualEntity = conceptualEntity.Properties.Find(x => x.Name == complex.Name).ComplexEntity;

                // Map column
                foreach (ScalarProperty itemColumn in complex.ScalarProperties)
                {
                    ConceptualProperty conceptualColumn = conceptualEntity.Properties.Find(x => x.Name == itemColumn.Name);
                    StorageColumn storageColumn = storageTable.Columns.Find(x => x.Name == itemColumn.ColumnName);

                    if (conceptualColumn != null)
                    {
                        conceptualColumn.Mapping = storageColumn;
                        if (storageColumn.Mappings == null)
                        {
                            storageColumn.Mappings = new List<ConceptualProperty>();
                        }

                        storageColumn.Mappings.Add(conceptualColumn);
                    }
                    else
                    {
                        // Must be complex / associated?!
                        List<ConceptualEntity> linkedModel = FindAllInheritEntity(conceptualEntity);
                        List<ConceptualProperty> conceptualProperties = linkedModel.SelectMany(y => y.Properties.Where(x => x.Name == itemColumn.Name)).ToList();
                        if (conceptualProperties.Count == 0)
                        {
                            throw new Exception("Property not found!");
                        }
                        conceptualProperties.ForEach(x => x.Mapping = storageColumn);
                        if (storageColumn.Mappings == null)
                        {
                            storageColumn.Mappings = new List<ConceptualProperty>();
                        }
                        storageColumn.Mappings.AddRange(conceptualProperties);
                    }
                }

                BuildMappingComplex(model, storageTable, conceptualEntity, complex.ComplexProperties);
            }
        }

        /// <summary>
        ///     Searches for all inherit entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The found all inherit entity.</returns>
        internal static List<ConceptualEntity> FindAllInheritEntity(ConceptualEntity entity)
        {
            List<ConceptualEntity> entities = new List<ConceptualEntity>();

            while (entity.BaseEntity != null)
            {
                entities.Add(entity.BaseEntity);
                entity = entity.BaseEntity;
            }

            return entities;
        }
    }
}