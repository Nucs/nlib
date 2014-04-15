// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Z.EntityFramework.Model;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DatabaseExtension
    {
        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType, SqlTransaction transaction) where T : new()
        {
            Model model = @this.GetDbContext().GetModel();
            ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == typeof (T).Name);
            IEnumerable<Tuple<string, string>> mapping = entity.Mapping.Columns.Select(x => new Tuple<string, string>(x.Name, x.Mapping.Name));

            if (entity == null)
            {
                throw new Exception(string.Format("Entity:{0} not found in DbContext.", typeof (T).Name));
            }

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
                    reader.Read();
                    return reader.ToEntity<T>(mapping);
                }
            }
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="commandFactory">The command factory.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, Action<SqlCommand> commandFactory) where T : new()
        {
            Model model = @this.GetDbContext().GetModel();
            ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == typeof (T).Name);
            IEnumerable<Tuple<string, string>> mapping = entity.Mapping.Columns.Select(x => new Tuple<string, string>(x.Name, x.Mapping.Name));

            if (entity == null)
            {
                throw new Exception(string.Format("Entity:{0} not found in DbContext.", typeof (T).Name));
            }

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
                    reader.Read();
                    return reader.ToEntity<T>(mapping);
                }
            }
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, null, CommandType.Text, null);
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, SqlTransaction transaction) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, null, CommandType.Text, transaction);
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, CommandType commandType) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, null, commandType, null);
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, CommandType commandType, SqlTransaction transaction) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, null, commandType, transaction);
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, SqlParameter[] parameters) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, parameters, CommandType.Text, null);
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, SqlParameter[] parameters, SqlTransaction transaction) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, parameters, CommandType.Text, transaction);
        }

        /// <summary>
        ///     A Database extension method that SQL execute entity with mapping.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A T.</returns>
        public static T SqlExecuteEntityWithMapping<T>(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType) where T : new()
        {
            return @this.SqlExecuteEntityWithMapping<T>(cmdText, parameters, commandType, null);
        }
    }
}