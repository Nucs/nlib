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

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DbContexttExtension
    {
        /// <summary>
        ///     A DbContext extension method that gets database first model.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The database first model.</returns>
        public static Model GetDatabaseFirstModel(this DbContext @this)
        {
            MemoryCache cache = MemoryCache.Default;

            if (cache.Contains(@this.ToString()))
            {
                return (Model) cache["GetDatabaseFirstModel;" + @this];
            }

            var model = new Model();

            SetStorageModel(@this, model);
            SetConceptualModel(@this, model);
            SetMappingModel(@this, model);

            cache.Add(new CacheItem("GetDatabaseFirstModel;" + @this, model), new CacheItemPolicy());

            return model;
        }

        /// <summary>
        ///     Sets storage model.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="model">The model.</param>
        private static void SetStorageModel(DbContext context, Model model)
        {
            const string space = "{http://schemas.microsoft.com/ado/2009/11/edm/ssdl}";
            XNamespace xStore = "http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator";
            XDocument xml = XDocument.Parse(context.GetStorageModelString());

            if (xml.Root == null)
            {
                throw new Exception("Invalid Xml file.");
            }

            var storage = new StorageModel();
            model.Storage = storage;
            model.Storage.Name = context.Database.Connection.Database;
            var xmlData = new
                {
                    EntityTypes = xml.Root.Elements(space + "EntityType").Select(x => new
                        {
                            StorageTable = new StorageTable
                                {
                                    Name = x.Attribute("Name").Value,
                                    Columns = (from y in x.Elements(space + "Property")
                                               select new StorageColumn
                                                   {
                                                       Name = y.Attribute("Name").Value,
                                                       Type = y.Attribute("Type").Value,
                                                       IsNullable = (y.Attribute("Nullable") == null || Convert.ToBoolean(y.Attribute("Nullable").Value)),
                                                       MaxLength = y.Attribute("MaxLength") != null ? Convert.ToInt32(y.Attribute("MaxLength").Value) : (int?) null,
                                                       Precision = y.Attribute("Precision") != null ? Convert.ToInt32(y.Attribute("Precision").Value) : (int?) null,
                                                       Scale = y.Attribute("Scale") != null ? Convert.ToInt32(y.Attribute("Scale").Value) : (int?) null,
                                                       IsIdentity = y.Attribute("StoreGeneratedPattern") != null && y.Attribute("StoreGeneratedPattern").Value == "Identity",
                                                       IsComputed = y.Attribute("StoreGeneratedPattern") != null && y.Attribute("StoreGeneratedPattern").Value == "Computed",
                                                   }
                                              ).ToList(),
                                },
                            PrimaryKeyColumn = (from y in x.Elements(space + "Key").Elements(space + "PropertyRef")
                                                select y.Attribute("Name").Value
                                               ).ToList()
                        }).ToList(),
                    Functions = xml.Root.Elements(space + "Function").Select(x => new
                        {
                            StorageFunction = new StorageFunction
                                {
                                    Name = x.Attribute("Name").Value,
                                    Aggregate = x.Attribute("Nullable") != null && Convert.ToBoolean(x.Attribute("Nullable").Value),
                                    BuiltIn = x.Attribute("Nullable") != null && Convert.ToBoolean(x.Attribute("BuiltIn").Value),
                                    NiladicFunction = x.Attribute("Nullable") != null && Convert.ToBoolean(x.Attribute("NiladicFunction").Value),
                                    IsComposable = x.Attribute("Nullable") != null && Convert.ToBoolean(x.Attribute("IsComposable").Value),
                                    ParameterTypeSemantics = x.Attribute("ParameterTypeSemantics") != null ? x.Attribute("ParameterTypeSemantics").Value : "",
                                    Schema = x.Attribute("Schema") != null ? x.Attribute("Schema").Value : "",
                                    Parameters = (from y in x.Elements(space + "Parameter")
                                                  select new StorageFunctionParameter
                                                      {
                                                          Name = y.Attribute("Name").Value,
                                                          Type = y.Attribute("Type").Value,
                                                          Mode = y.Attribute("Mode").Value
                                                      }).ToList()
                                }
                        }).ToList(),
                    EntityContainer = new
                        {
                            Name = xml.Root.Element(space + "EntityContainer").Attribute("Name"),
                            EntitySet = xml.Root.Element(space + "EntityContainer").Elements(space + "EntitySet")
                                           .Select(x => new
                                               {
                                                   Name = x.Attribute("Name").Value,
                                                   EntityType = x.Attribute("EntityType").Value,
                                                   Schema = x.Attribute("Schema") != null ? x.Attribute("Schema").Value : x.Attribute(xStore + "Schema").Value,
                                                   Type = x.Attribute(xStore + "Type").Value
                                               }).ToList(),
                        }
                };

            // Set Primary Key
            foreach (var item in xmlData.EntityTypes)
            {
                item.StorageTable.PrimaryKey = new StoragePrimaryKey
                    {
                        Columns = item.StorageTable.Columns.Where(x => item.PrimaryKeyColumn.Any(y => y == x.Name)).ToList()
                    };
                item.StorageTable.PrimaryKey.Columns.ForEach(x => x.IsPrimaryKey = true);
            }

            foreach (var item in xmlData.EntityTypes)
            {
                var entity = xmlData.EntityContainer.EntitySet.Find(x => x.Name == item.StorageTable.Name);
                item.StorageTable.Schema = entity.Schema;
                item.StorageTable.Type = entity.Type;
            }

            storage.Tables = xmlData.EntityTypes.Select(x => x.StorageTable).ToList();
            storage.Functions = xmlData.Functions.Select(x => x.StorageFunction).ToList();
        }

        /// <summary>
        ///     Sets conceptual model.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="context">The context.</param>
        /// <param name="model">The model.</param>
        private static void SetConceptualModel(DbContext context, Model model)
        {
            const string space = "{http://schemas.microsoft.com/ado/2009/11/edm}";
            XDocument xml = XDocument.Parse(context.GetConceptualModelString());

            if (xml.Root == null)
            {
                throw new Exception("Invalid Xml file.");
            }

            var conceptual = new ConceptualModel();
            model.Conceptual = conceptual;
            conceptual.Name = xml.Root.Attribute("Namespace").Value;

            var xmlData = new
                {
                    EntityTypes = xml.Root.Elements(space + "EntityType").Select(x => new
                        {
                            ConceptualEntity = new ConceptualEntity
                                {
                                    Name = x.Attribute("Name").Value,
                                    Properties = (from y in x.Elements(space + "Property")
                                                  select new ConceptualProperty
                                                      {
                                                          Name = y.Attribute("Name").Value,
                                                          Type = y.Attribute("Type").Value,
                                                          IsNullable = (y.Attribute("Nullable") == null || Convert.ToBoolean(y.Attribute("Nullable").Value)),
                                                          MaxLength = (y.Attribute("MaxLength") != null && y.Attribute("MaxLength").Value != "Max" ? Convert.ToInt32(y.Attribute("MaxLength").Value) : (int?) null),
                                                          IsMaxLength = y.Attribute("IsMaxLength") != null && Convert.ToBoolean(y.Attribute("IsMaxLength").Value),
                                                          Precision = (y.Attribute("Precision") != null ? Convert.ToInt32(y.Attribute("Precision").Value) : (int?) null),
                                                          Scale = (y.Attribute("Scale") != null ? Convert.ToInt32(y.Attribute("Scale").Value) : (int?) null),
                                                          IsUnicode = y.Attribute("Unicode") != null && Convert.ToBoolean(y.Attribute("Unicode").Value),
                                                          IsFixedLength = y.Attribute("FixedLength") == null || Convert.ToBoolean(y.Attribute("FixedLength").Value)
                                                      }
                                                 ).ToList(),
                                },
                            PrimaryKeyColumn = (from y in x.Elements(space + "Key").Elements(space + "PropertyRef")
                                                select y.Attribute("Name").Value
                                               ).ToList()
                        }).ToList(),
                    EntityContainer = new
                        {
                            Name = xml.Root.Element(space + "EntityContainer").Attribute("Name"),
                            EntitySet = xml.Root.Element(space + "EntityContainer").Elements(space + "EntitySet").Select(x => new
                                {
                                    Name = x.Attribute("Name").Value,
                                    EntityType = x.Attribute("EntityType").Value,
                                }).ToList(),
                            Functions = xml.Root.Element(space + "EntityContainer").Elements(space + "FunctionImport").Select(x => new
                                {
                                    ConceptualFunction = new ConceptualFunction
                                        {
                                            Name = x.Attribute("Name").Value,
                                            ReturnType = x.Attribute("ReturnType") != null ? x.Attribute("ReturnType").Value : "",
                                            Parameters = (from y in x.Elements(space + "Parameter")
                                                          select new ConceptualFunctionParameter
                                                              {
                                                                  Name = y.Attribute("Name").Value,
                                                                  Type = y.Attribute("Type").Value,
                                                                  Mode = y.Attribute("Mode").Value
                                                              }).ToList()
                                        }
                                }).ToList(),
                        }
                };

            conceptual.Entities = xmlData.EntityTypes.Select(x => x.ConceptualEntity).ToList();
            conceptual.Functions = xmlData.EntityContainer.Functions.Select(x => x.ConceptualFunction).ToList();

            // Set Key
            foreach (var item in xmlData.EntityTypes)
            {
                item.ConceptualEntity.Key = new ConceptualKey
                    {
                        Properties = item.ConceptualEntity.Properties.Where(x => item.PrimaryKeyColumn.Any(y => y == x.Name)).ToList()
                    };
                item.ConceptualEntity.Key.Properties.ForEach(x => x.IsPrimaryKey = true);
            }
            // Set conceptualSetData
            foreach (var item in xmlData.EntityTypes)
            {
                var entity = xmlData.EntityContainer.EntitySet.Find(x => x.EntityType.EndsWith("." + item.ConceptualEntity.Name));
                item.ConceptualEntity.Type = entity.EntityType;
                item.ConceptualEntity.SetName = entity.Name;
            }
        }

        /// <summary>
        ///     Sets mapping model.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="model">The model.</param>
        private static void SetMappingModel(DbContext context, Model model)
        {
            const string mappingNamespace = "{http://schemas.microsoft.com/ado/2009/11/mapping/cs}";


            XDocument mappingXml = XDocument.Parse(context.GetMappingModelString());

            // Mapping
            var mappingData = (from x in mappingXml.Root.Element(mappingNamespace + "EntityContainerMapping").Elements(mappingNamespace + "EntitySetMapping")
                               select new
                                   {
                                       ConceptualSetName = x.Attribute("Name").Value,
                                       ConceptualNamespace = x.Element(mappingNamespace + "EntityTypeMapping").Attribute("TypeName").Value,
                                       StorageName = x.Element(mappingNamespace + "EntityTypeMapping").Element(mappingNamespace + "MappingFragment").Attribute("StoreEntitySet").Value,
                                       MappingProperties = x.Element(mappingNamespace + "EntityTypeMapping").Element(mappingNamespace + "MappingFragment").Elements(mappingNamespace + "ScalarProperty")
                                                            .Select(y => new
                                                                {
                                                                    ConceptualName = y.Attribute("Name").Value,
                                                                    StorageName = y.Attribute("ColumnName").Value
                                                                }).ToList()
                                   }).ToList();

            // Mapping function
            var mappingFunction = (from x in mappingXml.Root.Element(mappingNamespace + "EntityContainerMapping").Elements(mappingNamespace + "FunctionImportMapping")
                                   select new
                                       {
                                           FunctionImportName = x.Attribute("FunctionImportName").Value,
                                           FunctionName = x.Attribute("FunctionName").Value
                                       }).ToList();

            // Map
            foreach (var item in mappingData)
            {
                // Map table
                ConceptualEntity conceptualTable = model.Conceptual.Entities.Find(x => x.SetName == item.ConceptualSetName);
                StorageTable storageTable = model.Storage.Tables.Find(x => x.Name == item.StorageName);

                conceptualTable.Mappings.Add(storageTable);
                storageTable.Mappings.Add(conceptualTable);

                // Map column
                foreach (var itemColumn in item.MappingProperties)
                {
                    ConceptualProperty conceptualColumn = conceptualTable.Properties.Find(x => x.Name == itemColumn.ConceptualName);
                    StorageColumn storageColumn = storageTable.Columns.Find(x => x.Name == itemColumn.StorageName);
                    conceptualColumn.Mapping = storageColumn;
                    if (storageColumn.Mappings == null)
                    {
                        storageColumn.Mappings = new List<ConceptualProperty>();
                    }

                    storageColumn.Mappings.Add(conceptualColumn);
                }
            }

            foreach (var item in mappingFunction)
            {
                string functionName = item.FunctionName.Substring(item.FunctionName.IndexOf(".Store.") + 7);
                ConceptualFunction conceptualFunction = model.Conceptual.Functions.Find(x => x.Name == item.FunctionImportName);
                StorageFunction storageFunction = model.Storage.Functions.Find(x => x.Name == functionName);

                conceptualFunction.Mapping = storageFunction;
                storageFunction.Mapping = conceptualFunction;

                // Map parameters (We assume they always have the same name)
                foreach (ConceptualFunctionParameter param in conceptualFunction.Parameters)
                {
                    StorageFunctionParameter storageFunctionParameter = storageFunction.Parameters.Find(x => x.Name == param.Name);

                    param.Mapping = storageFunctionParameter;
                    storageFunctionParameter.Mapping = param;
                }
            }
        }
    }
}