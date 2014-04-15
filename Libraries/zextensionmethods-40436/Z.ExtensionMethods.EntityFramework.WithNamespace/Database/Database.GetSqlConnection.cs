// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Data.SqlClient;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DatabaseExtension
    {
        /// <summary>
        ///     A Database extension method that gets SQL connection.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The SQL connection.</returns>
        public static SqlConnection GetSqlConnection(this Database @this)
        {
            return new SqlConnection(@this.Connection.ConnectionString);
        }
    }
}