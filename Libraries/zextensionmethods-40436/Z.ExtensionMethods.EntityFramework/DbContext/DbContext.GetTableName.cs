// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Text.RegularExpressions;

public static partial class DbContexttExtension
{
    /// <summary>
    ///     A DbContext extension method that gets table name.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The table name.</returns>
    public static string GetTableName<T>(this DbContext @this) where T : class
    {
        ObjectContext context = @this.GetObjectContext();
        string sql = context.CreateObjectSet<T>().ToTraceString();
        var regex = new Regex("FROM (?<table>.*) AS");
        Match match = regex.Match(sql);

        string table = match.Groups["table"].Value;
        return table;
    }
}