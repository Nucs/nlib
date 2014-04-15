// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Z.EntityFramework.Model;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class Extension
    {
        /// <summary>
        ///     Gets the SQL bulk column mappings in this collection.
        /// </summary>
        /// <param name="context">The context to act on.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to get the SQL bulk column mappings in this collection.
        /// </returns>
        public static IEnumerable<SqlBulkCopyColumnMapping> GetSqlBulkColumnMapping(this DbContext context, Type type)
        {
            var model = context.GetModel();
            var conceptual = model.Conceptual.Entities.Find(x => x.Name == type.Name);
            var mappings = new List<SqlBulkCopyColumnMapping>();
            GetSqlBulkColumnMapping_AddMapping(mappings, conceptual, new List<ConceptualEntity>());

            return mappings;
        }

        /// <summary>
        ///     Gets SQL bulk column mapping add mapping.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="parentEntities">The parent entities.</param>
        internal static void GetSqlBulkColumnMapping_AddMapping(List<SqlBulkCopyColumnMapping> mappings, ConceptualEntity entity, List<ConceptualEntity> parentEntities)
        {
            foreach (var property in entity.Properties)
            {
                if (property.ComplexEntity != null)
                {
                    var newParentEntities = parentEntities.ToList();
                    newParentEntities.Add(entity);
                    GetSqlBulkColumnMapping_AddMapping(mappings, property.ComplexEntity, newParentEntities);
                }
                else
                {
                    if (parentEntities.Count > 0)
                    {
                        var parentNamespace = string.Join(".", parentEntities.Select(x => x.Name));
                        var columnName = string.Concat(parentNamespace, ".", property.Name);
                        if (!mappings.Exists(x => x.SourceColumn == columnName))
                        {
                            mappings.Add(new SqlBulkCopyColumnMapping(columnName, property.Mapping.Name));
                        }
                    }
                    else
                    {
                        mappings.Add(new SqlBulkCopyColumnMapping(property.Name, property.Mapping.Name));
                    }
                }
            }

            if (entity.BaseEntity != null)
            {
                var newParentEntities = parentEntities.ToList();
                newParentEntities.Add(entity);
                GetSqlBulkColumnMapping_AddMapping(mappings, entity.BaseEntity, newParentEntities);
            }
        }
    }
}