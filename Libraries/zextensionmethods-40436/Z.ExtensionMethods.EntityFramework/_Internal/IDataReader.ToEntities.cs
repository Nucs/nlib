// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Z.EntityFramework.Model;
using Z.ExtensionMethods.Object;

namespace Z.ExtensionMethods
{
    internal static partial class IDataReaderExtension
    {
        /// <summary>
        ///     Enumerates to entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to an IEnumerable&lt;T&gt;</returns>
        internal static IEnumerable<T> ToEntities<T>(this IDataReader @this) where T : new()
        {
            Type type = typeof (T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var list = new List<T>();

            var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
                                                     .Select(@this.GetName));

            while (@this.Read())
            {
                var entity = new T();

                foreach (PropertyInfo property in properties)
                {
                    if (hash.Contains(property.Name))
                    {
                        Type valueType = property.PropertyType;
                        property.SetValue(entity, @this[property.Name].To(valueType), null);
                    }
                }

                foreach (FieldInfo field in fields)
                {
                    if (hash.Contains(field.Name))
                    {
                        Type valueType = field.FieldType;
                        field.SetValue(entity, @this[field.Name].To(valueType));
                    }
                }

                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        ///     Enumerates to entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="mappings">The mappings.</param>
        /// <returns>The given data converted to an IEnumerable&lt;T&gt;</returns>
        internal static IEnumerable<T> ToEntities<T>(this IDataReader @this, IEnumerable<Tuple<string, string>> mappings) where T : new()
        {
            Type type = typeof (T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var list = new List<T>();

            var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
                                                     .Select(@this.GetName));

            Dictionary<string, string> dictMapping = mappings.ToDictionary(tuple => tuple.Item2, tuple => tuple.Item1);

            while (@this.Read())
            {
                var entity = new T();

                foreach (PropertyInfo property in properties)
                {
                    if (dictMapping.ContainsKey(property.Name) && hash.Contains(dictMapping[property.Name]))
                    {
                        Type valueType = property.PropertyType;
                        property.SetValue(entity, @this[dictMapping[property.Name]].To(valueType), null);
                    }
                }

                foreach (FieldInfo field in fields)
                {
                    if (dictMapping.ContainsKey(field.Name) && hash.Contains(dictMapping[field.Name]))
                    {
                        Type valueType = field.FieldType;
                        field.SetValue(entity, @this[dictMapping[field.Name]].To(valueType));
                    }
                }

                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        ///     Enumerates to entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="conceptualEntity">The conceptual entity.</param>
        /// <returns>The given data converted to an IEnumerable&lt;T&gt;</returns>
        internal static IEnumerable<T> ToEntities<T>(this IDataReader @this, ConceptualEntity conceptualEntity) where T : new()
        {
            var list = new List<T>();
            while (@this.Read())
            {
                var entity = new T();
                ToEntities_AddEntities(entity, @this, conceptualEntity);
                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        ///     Converts this object to the entities add entities.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="dr">The dr.</param>
        /// <param name="entity">The entity.</param>
        internal static void ToEntities_AddEntities<T>(T item, IDataReader dr, ConceptualEntity entity)
        {
            foreach (ConceptualProperty property in entity.Properties)
            {
                if (property.ComplexEntity != null)
                {
                    object complexItem = item.GetType().GetProperty(property.Name).GetValue(item, null);
                    if (complexItem == null)
                    {
                        PropertyInfo propertyComplex = item.GetType().GetProperty(property.Name);
                        complexItem = Activator.CreateInstance(propertyComplex.PropertyType);
                        propertyComplex.SetValue(item, complexItem, null);
                    }
                    ToEntities_AddEntities(complexItem, dr, property.ComplexEntity);
                }
                else
                {
                    object value = dr[property.Mapping.Name];
                    PropertyInfo propertyItem = item.GetType().GetProperty(property.Name);
                    propertyItem.SetValue(item, value.To(propertyItem.PropertyType), null);
                }
            }

            if (entity.BaseEntity != null)
            {
                ToEntities_AddEntities(item, dr, entity.BaseEntity);
            }
        }
    }
}