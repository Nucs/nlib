// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

public static partial class IDictionaryExtension
{
    /// <summary>
    ///     An IDictionary&lt;string,object&gt; extension method that converts the @this to a SQL parameters.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a SqlParameter[].</returns>
    public static SqlParameter[] ToSqlParameters(this IDictionary<string, object> @this)
    {
        return @this.Select(x => new SqlParameter(x.Key, x.Value)).ToArray();
    }
}