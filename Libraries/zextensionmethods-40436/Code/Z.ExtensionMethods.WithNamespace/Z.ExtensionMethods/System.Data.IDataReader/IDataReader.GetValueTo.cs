// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;
using Z.ExtensionMethods.Object;

namespace Z.ExtensionMethods
{
    public static partial class IDataReaderExtension
    {
        /// <summary>
        ///     An IDataReader extension method that gets value to.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="index">Zero-based index of the.</param>
        /// <returns>The value to.</returns>
        public static T GetValueTo<T>(this IDataReader @this, int index)
        {
            return @this.GetValue(index).To<T>();
        }

        /// <summary>
        ///     An IDataReader extension method that gets value to.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The value to.</returns>
        public static T GetValueTo<T>(this IDataReader @this, string columnName)
        {
            return @this.GetValue(@this.GetOrdinal(columnName)).To<T>();
        }
    }
}