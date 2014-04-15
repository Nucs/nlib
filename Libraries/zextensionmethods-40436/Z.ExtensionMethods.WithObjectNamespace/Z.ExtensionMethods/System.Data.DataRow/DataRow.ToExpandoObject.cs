// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data;
using System.Dynamic;

public static partial class DataRowExtension
{
    /// <summary>
    ///     A DataRow extension method that converts the @this to an expando objects.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a dynamic.</returns>
    /// ###
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public static dynamic ToExpandoObject(this DataRow @this)
    {
        dynamic entity = new ExpandoObject();
        var expandoDict = (IDictionary<string, object>) entity;

        foreach (DataColumn column in @this.Table.Columns)
        {
            expandoDict.Add(column.ColumnName, @this[column]);
        }

        return expandoDict;
    }
}