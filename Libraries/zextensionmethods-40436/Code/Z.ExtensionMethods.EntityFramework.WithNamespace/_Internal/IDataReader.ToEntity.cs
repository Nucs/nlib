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
        ///     An IDataReader extension method that converts this object to an entity.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The given data converted to a T.</returns>
        internal static T ToEntity<T>(this IDataReader @this) where T : new()
        {
            Type type = typeof (T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var entity = new T();

            var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
                                                     .Select(@this.GetName));

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

            return entity;
        }

        /// <summary>
        ///     An IDataReader extension method that converts this object to an entity.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="mappings">The mappings.</param>
        /// <returns>The given data converted to a T.</returns>
        internal static T ToEntity<T>(this IDataReader @this, IEnumerable<Tuple<string, string>> mappings) where T : new()
        {
            Type type = typeof (T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var entity = new T();

            var hash = new HashSet<string>(Enumerable.Range(0, @this.FieldCount)
                                                     .Select(@this.GetName));

            Dictionary<string, string> dictMapping = mappings.ToDictionary(tuple => tuple.Item2, tuple => tuple.Item1);

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

            return entity;
        }

        /// <summary>
        ///     An IDataReader extension method that converts this object to an entity.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="conceptualEntity">The conceptual entity.</param>
        /// <returns>The given data converted to a T.</returns>
        internal static T ToEntity<T>(this IDataReader @this, ConceptualEntity conceptualEntity) where T : new()
        {
            var entity = new T();
            ToEntities_AddEntities(entity, @this, conceptualEntity);
            return entity;
        }
    }
}