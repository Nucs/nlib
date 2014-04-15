// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

namespace Z.ExtensionMethods
{
    public static partial class IDataReaderExtension
    {
        /// <summary>
        ///     An IDataReader extension method that converts the @this to a data table.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DataTable.</returns>
        public static DataTable ToDataTable(this IDataReader @this)
        {
            var dt = new DataTable();
            dt.Load(@this);
            return dt;
        }
    }
}