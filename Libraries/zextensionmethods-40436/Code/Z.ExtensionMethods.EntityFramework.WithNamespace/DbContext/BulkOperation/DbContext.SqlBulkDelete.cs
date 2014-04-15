// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Z.EntityFramework.Model;
using Z.Utility;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DbContexttExtension
    {
        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="copyOptions">Options for controlling the copy.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sqlBulkCopyFactory">The SQL bulk copy factory.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, SqlBulkCopyOptions copyOptions, SqlTransaction transaction, Action<SqlBulkCopy> sqlBulkCopyFactory)
        {
            Model model = @this.GetModel();
            ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == (typeof (T).Name));

            if (entity == null)
            {
                throw new Exception("Entity not found in model.");
            }
            if (entity.IsTPT)
            {
                throw new Exception("EF TPT (Table per type) is not supported for bulk operation");
            }

            DbConnection connection = @this.Database.Connection;

            if (@this.Database.Connection.State == ConnectionState.Closed)
            {
                @this.Database.Connection.Open();
            }
            using (var sqlBulkCopy = new SqlBulkCopy((SqlConnection) connection, copyOptions, transaction))
            {
                if (sqlBulkCopyFactory != null)
                {
                    sqlBulkCopyFactory(sqlBulkCopy);
                }

                if (sqlBulkCopy.ColumnMappings.Count == 0)
                {
                    IEnumerable<SqlBulkCopyColumnMapping> mapping = @this.GetSqlBulkColumnMapping(typeof (T));
                    mapping.ToList().ForEach(x => sqlBulkCopy.ColumnMappings.Add(x));
                }
                if (string.IsNullOrEmpty(sqlBulkCopy.DestinationTableName))
                {
                    sqlBulkCopy.DestinationTableName = string.IsNullOrEmpty(entity.Mapping.Schema)
                                                           ? string.Concat("[" + entity.Mapping.Name + "]")
                                                           : string.Concat("[" + entity.Mapping.Schema + "].[" + entity.Mapping.Name + "]");
                }

                var bulkOperation = new SqlBulkOperation
                    {
                        SqlBulkCopy = sqlBulkCopy,
                        DataSource = @this.ToDataTable(entities),
                        PrimaryKeys = entity.Mapping.PrimaryKey.Columns.Select(x => x.Name).ToList()
                    };
                bulkOperation.BulkDelete();
            }
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="sqlBulkCopyFactory">The SQL bulk copy factory.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, Action<SqlBulkCopy> sqlBulkCopyFactory)
        {
            @this.SqlBulkDelete(entities, SqlBulkCopyOptions.Default, null, sqlBulkCopyFactory);
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sqlBulkCopyFactory">The SQL bulk copy factory.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, SqlTransaction transaction, Action<SqlBulkCopy> sqlBulkCopyFactory)
        {
            @this.SqlBulkDelete(entities, SqlBulkCopyOptions.Default, transaction, sqlBulkCopyFactory);
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="sqlBulkCopyFactory">The SQL bulk copy factory.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, DbTransaction dbTransaction, Action<SqlBulkCopy> sqlBulkCopyFactory)
        {
            var entityTransaction = dbTransaction.GetType().GetField("_entityTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbTransaction) as EntityTransaction;
            var transaction = entityTransaction.GetType().GetField("_storeTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entityTransaction) as SqlTransaction;
            @this.SqlBulkDelete(entities, SqlBulkCopyOptions.Default, transaction, sqlBulkCopyFactory);
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="copyOptions">Options for controlling the copy.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="sqlBulkCopyFactory">The SQL bulk copy factory.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, SqlBulkCopyOptions copyOptions, DbTransaction dbTransaction, Action<SqlBulkCopy> sqlBulkCopyFactory)
        {
            var entityTransaction = dbTransaction.GetType().GetField("_entityTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbTransaction) as EntityTransaction;
            var transaction = entityTransaction.GetType().GetField("_storeTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entityTransaction) as SqlTransaction;
            @this.SqlBulkDelete(entities, copyOptions, transaction, sqlBulkCopyFactory);
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities)
        {
            @this.SqlBulkDelete(entities, SqlBulkCopyOptions.Default, null, copy => { });
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="transaction">The transaction.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, SqlTransaction transaction)
        {
            @this.SqlBulkDelete(entities, SqlBulkCopyOptions.Default, transaction, copy => { });
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="copyOptions">Options for controlling the copy.</param>
        /// <param name="transaction">The transaction.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, SqlBulkCopyOptions copyOptions, SqlTransaction transaction)
        {
            @this.SqlBulkDelete(entities, copyOptions, transaction, copy => { });
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, DbContextTransaction dbTransaction)
        {
            var entityTransaction = dbTransaction.GetType().GetField("_entityTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbTransaction) as EntityTransaction;
            var transaction = entityTransaction.GetType().GetField("_storeTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entityTransaction) as SqlTransaction;
            @this.SqlBulkDelete(entities, SqlBulkCopyOptions.Default, transaction, copy => { });
        }

        /// <summary>
        ///     A DbContext extension method that SQL bulk delete.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="copyOptions">Options for controlling the copy.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        public static void SqlBulkDelete<T>(this DbContext @this, IEnumerable<T> entities, SqlBulkCopyOptions copyOptions, DbContextTransaction dbTransaction)
        {
            var entityTransaction = dbTransaction.GetType().GetField("_entityTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbTransaction) as EntityTransaction;
            var transaction = entityTransaction.GetType().GetField("_storeTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entityTransaction) as SqlTransaction;
            @this.SqlBulkDelete(entities, copyOptions, transaction, copy => { });
        }
    }
}