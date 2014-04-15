// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Model;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class Extension
    {
        /// <summary>
        ///     A DbContext extension method that converts this object to a data table.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="context">The context to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>The given data converted to a DataTable.</returns>
        public static DataTable ToDataTable<T>(this DbContext context, IEnumerable<T> entities)
        {
            var model = context.GetModel();
            var conceptual = model.Conceptual.Entities.Find(x => x.Name == typeof (T).Name);

            var dt = new DataTable();

            ToDataTable_AddColumn(dt, conceptual, new List<ConceptualEntity>());

            var isTypeAdded = false;
            foreach (var entity in entities)
            {
                if (!isTypeAdded)
                {
                    ToDataTable_AddType(dt, entity, conceptual, entity.GetType(), new List<ConceptualEntity>());
                    isTypeAdded = true;
                }
                var dr = dt.NewRow();
                ToDataTable_AddRow(dr, entity, conceptual, entity.GetType(), new List<ConceptualEntity>());
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        ///     Converts this object to a data table add column.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="parentEntities">The parent entities.</param>
        internal static void ToDataTable_AddColumn(DataTable dt, ConceptualEntity entity, List<ConceptualEntity> parentEntities)
        {
            foreach (var property in entity.Properties)
            {
                if (property.ComplexEntity != null)
                {
                    var newParentEntities = parentEntities.ToList();
                    newParentEntities.Add(entity);
                    ToDataTable_AddColumn(dt, property.ComplexEntity, newParentEntities);
                }
                else
                {
                    if (parentEntities.Count > 0)
                    {
                        var parentNamespace = string.Join(".", parentEntities.Select(x => x.Name));
                        var columnName = string.Concat(parentNamespace, ".", property.Name);
                        if (!dt.Columns.Contains(columnName))
                        {
                            dt.Columns.Add(columnName);
                        }
                    }
                    else
                    {
                        dt.Columns.Add(property.Name);
                    }
                }
            }

            if (entity.BaseEntity != null)
            {
                var newParentEntities = parentEntities.ToList();
                newParentEntities.Add(entity);
                ToDataTable_AddColumn(dt, entity.BaseEntity, newParentEntities);
            }
        }

        /// <summary>
        ///     Converts this object to a data table add row.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="item">The item.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="type">The type.</param>
        /// <param name="parentEntities">The parent entities.</param>
        internal static void ToDataTable_AddRow(DataRow dr, object item, ConceptualEntity entity, Type type, List<ConceptualEntity> parentEntities)
        {
            foreach (var property in entity.Properties)
            {
                if (property.ComplexEntity != null)
                {
                    var newParentEntities = parentEntities.ToList();
                    newParentEntities.Add(entity);
                    var newItem = type.GetProperty(property.Name).GetValue(item, null);
                    var newType = newItem.GetType();
                    ToDataTable_AddRow(dr, newItem, property.ComplexEntity, newType, newParentEntities);
                }
                else
                {
                    if (parentEntities.Count > 0)
                    {
                        var parentNamespace = string.Join(".", parentEntities.Select(x => x.Name));
                        var columnName = string.Concat(parentNamespace, ".", property.Name);

                        var value = type.GetProperty(property.Name).GetValue(item, null);

                        dr[columnName] = value ?? DBNull.Value;
                    }
                    else
                    {
                        var columnName = property.Name;
                        var value = type.GetProperty(property.Name).GetValue(item, null);
                        dr[columnName] = value ?? DBNull.Value;
                    }
                }
            }

            if (entity.BaseEntity != null)
            {
                var newParentEntities = parentEntities.ToList();
                newParentEntities.Add(entity);
                ToDataTable_AddRow(dr, item, entity.BaseEntity, type, newParentEntities);
            }
        }

        /// <summary>
        ///     Converts this object to a data table add type.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="item">The item.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="type">The type.</param>
        /// <param name="parentEntities">The parent entities.</param>
        internal static void ToDataTable_AddType(DataTable dt, object item, ConceptualEntity entity, Type type, List<ConceptualEntity> parentEntities)
        {
            foreach (var property in entity.Properties)
            {
                if (property.ComplexEntity != null)
                {
                    var newParentEntities = parentEntities.ToList();
                    newParentEntities.Add(entity);
                    var newItem = type.GetProperty(property.Name).GetValue(item, null);
                    var newType = newItem.GetType();
                    ToDataTable_AddType(dt, newItem, property.ComplexEntity, newType, newParentEntities);
                }
                else
                {
                    if (parentEntities.Count > 0)
                    {
                        var parentNamespace = string.Join(".", parentEntities.Select(x => x.Name));
                        var columnName = string.Concat(parentNamespace, ".", property.Name);

                        var dataType = type.GetProperty(property.Name).PropertyType;
                        if (dataType.IsGenericType)
                        {
                            dataType = dataType.GetGenericArguments()[0];
                        }
                        dt.Columns[columnName].DataType = dataType;
                    }
                    else
                    {
                        var columnName = property.Name;

                        var dataType = type.GetProperty(property.Name).PropertyType;
                        if (dataType.IsGenericType)
                        {
                            dataType = dataType.GetGenericArguments()[0];
                        }
                        dt.Columns[columnName].DataType = dataType;
                    }
                }
            }

            if (entity.BaseEntity != null)
            {
                var newParentEntities = parentEntities.ToList();
                newParentEntities.Add(entity);
                ToDataTable_AddType(dt, item, entity.BaseEntity, type, newParentEntities);
            }
        }
    }
}