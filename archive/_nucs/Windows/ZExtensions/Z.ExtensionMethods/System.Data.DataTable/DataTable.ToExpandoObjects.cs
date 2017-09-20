// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

#if !(NET35 || NET3 || NET2)


using System.Collections.Generic;
using System.Data;
using System.Dynamic;

public static partial class DataTableExtension
{
    /// <summary>
    ///     Enumerates to expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as an IEnumerable&lt;dynamic&gt;</returns>
    /// ###
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public static IEnumerable<dynamic> ToExpandoObjects(this DataTable @this)
    {
        var list = new List<dynamic>();

        foreach (DataRow dr in @this.Rows)
        {
            dynamic entity = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>) entity;

            foreach (DataColumn column in @this.Columns)
            {
                expandoDict.Add(column.ColumnName, dr[column]);
            }


            list.Add(entity);
        }

        return list;
    }
}

#endif