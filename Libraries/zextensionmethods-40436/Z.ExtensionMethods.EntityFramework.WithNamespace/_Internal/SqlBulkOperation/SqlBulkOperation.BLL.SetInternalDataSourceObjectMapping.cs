// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>
        ///     Sets internal data source object mapping.
        /// </summary>
        public void SetInternalDataSourceObjectMapping()
        {
            var datasource = (DataSource as IEnumerable<object>);
            DataTable dt;

            bool isCustomMapping = false;
            if (ObjectMapping != null && ObjectMapping.Count > 0)
            {
                dt = new DataTable();

                // Select all column from the specified mapping.
                foreach (string column in ObjectMapping)
                {
                    dt.Columns.Add(column);
                }

                isCustomMapping = true;
            }
            else if (MappingList.Count > 0)
            {
                dt = new DataTable();

                MappingList.ForEach(x => dt.Columns.Add(x.SourceColumn));

                isCustomMapping = true;
            }
            else
            {
                // If the user have not specified which column to map, select all column from the table
                dt = GetTableColumn();
            }

            Type type = DataSource is object[] ? DataSource.GetType().GetElementType() : DataSource.GetType().GetGenericArguments()[0];
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => dt.Columns.Contains(x.Name)).ToArray();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(x => dt.Columns.Contains(x.Name)).ToArray();

            bool isTypeAdded = false;

            // Enumrate all items
            foreach (object item in datasource)
            {
                if (isCustomMapping && !isTypeAdded)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        var dataType = property.PropertyType;
                        if (dataType.IsGenericType)
                        {
                            dataType = dataType.GetGenericArguments()[0];
                        }
                        dt.Columns[property.Name].DataType = dataType;
                    }

                    foreach (FieldInfo field in fields)
                    {
                        var dataType = field.FieldType;
                        if (dataType.IsGenericType)
                        {
                            dataType = dataType.GetGenericArguments()[0];
                        }
                        dt.Columns[field.Name].DataType = dataType;
                    }

                    isTypeAdded = true;
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(item, null);
                    dr[property.Name] = value ?? DBNull.Value;
                }

                foreach (FieldInfo field in fields)
                {
                    var value = field.GetValue(item);
                    dr[field.Name] = value ?? DBNull.Value;
                }

                dt.Rows.Add(dr);
            }

            InternalDataSource = dt;
        }
    }
}