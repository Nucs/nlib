// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data;

public static partial class IDataReaderExtension
{
    /// <summary>
    ///     An IDataReader extension method that query if '@this' contains column.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="columnIndex">Zero-based index of the column.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool ContainsColumn(this IDataReader @this, int columnIndex)
    {
        try
        {
            return @this[columnIndex] != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    ///     An IDataReader extension method that query if '@this' contains column.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool ContainsColumn(this IDataReader @this, string columnName)
    {
        try
        {
            return @this[columnName] != null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}