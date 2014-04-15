// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Z.ExtensionMethods
{
    internal static partial class IDataReaderExtension
    {
        /// <summary>
        ///     An IDataReader extension method that converts the @this to an expando object.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a dynamic.</returns>
        internal static dynamic ToExpandoObject(this IDataReader @this)
        {
            Dictionary<int, KeyValuePair<int, string>> columnNames = Enumerable.Range(0, @this.FieldCount)
                                                                               .Select(x => new KeyValuePair<int, string>(x, @this.GetName(x)))
                                                                               .ToDictionary(pair => pair.Key);

            dynamic entity = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>) entity;

            Enumerable.Range(0, @this.FieldCount)
                      .ToList()
                      .ForEach(x => expandoDict.Add(columnNames[x].Value, @this[x]));

            return entity;
        }
    }
}