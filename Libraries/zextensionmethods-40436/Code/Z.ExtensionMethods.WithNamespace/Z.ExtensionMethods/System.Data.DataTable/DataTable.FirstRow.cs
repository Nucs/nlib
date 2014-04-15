// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

namespace Z.ExtensionMethods
{
    public static partial class DataTableExtension
    {
        /// <summary>
        ///     A DataTable extension method that return the first row.
        /// </summary>
        /// <param name="table">The table to act on.</param>
        /// <returns>The first row of the table.</returns>
        public static DataRow FirstRow(this DataTable table)
        {
            return table.Rows[0];
        }
    }
}