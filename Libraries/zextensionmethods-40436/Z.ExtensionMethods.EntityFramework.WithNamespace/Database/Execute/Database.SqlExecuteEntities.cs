// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DatabaseExtension
    {
        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType, SqlTransaction transaction) where T : new()
        {
            var connection = (SqlConnection) @this.Connection;

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = cmdText;
                command.CommandType = commandType;
                command.Transaction = transaction;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    return reader.ToEntities<T>();
                }
            }
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="commandFactory">The command factory.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, Action<SqlCommand> commandFactory) where T : new()
        {
            var connection = (SqlConnection) @this.Connection;

            using (SqlCommand command = connection.CreateCommand())
            {
                commandFactory(command);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    return reader.ToEntities<T>();
                }
            }
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, null, CommandType.Text, null);
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, SqlTransaction transaction) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, null, CommandType.Text, transaction);
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, CommandType commandType) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, null, commandType, null);
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, CommandType commandType, SqlTransaction transaction) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, null, commandType, transaction);
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, SqlParameter[] parameters) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, parameters, CommandType.Text, null);
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, SqlParameter[] parameters, SqlTransaction transaction) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, parameters, CommandType.Text, transaction);
        }

        /// <summary>
        ///     Enumerates SQL execute entities in this collection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL execute entities in this collection.
        /// </returns>
        public static IEnumerable<T> SqlExecuteEntities<T>(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType) where T : new()
        {
            return @this.SqlExecuteEntities<T>(cmdText, parameters, commandType, null);
        }
    }
}