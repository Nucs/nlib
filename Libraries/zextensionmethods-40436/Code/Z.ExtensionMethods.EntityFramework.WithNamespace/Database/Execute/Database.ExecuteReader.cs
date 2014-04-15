// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DatabaseExtension
    {
        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction)
        {
            DbConnection connection = @this.Connection;

            using (DbCommand command = connection.CreateCommand())
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

                return command.ExecuteReader();
            }
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="commandFactory">The command factory.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, Action<DbCommand> commandFactory)
        {
            DbConnection connection = @this.Connection;

            using (DbCommand command = connection.CreateCommand())
            {
                commandFactory(command);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return command.ExecuteReader();
            }
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText)
        {
            return @this.ExecuteReader(cmdText, null, CommandType.Text, null);
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, DbTransaction transaction)
        {
            return @this.ExecuteReader(cmdText, null, CommandType.Text, transaction);
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, CommandType commandType)
        {
            return @this.ExecuteReader(cmdText, null, commandType, null);
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, CommandType commandType, DbTransaction transaction)
        {
            return @this.ExecuteReader(cmdText, null, commandType, transaction);
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, DbParameter[] parameters)
        {
            return @this.ExecuteReader(cmdText, parameters, CommandType.Text, null);
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, DbParameter[] parameters, DbTransaction transaction)
        {
            return @this.ExecuteReader(cmdText, parameters, CommandType.Text, transaction);
        }

        /// <summary>
        ///     A Database extension method that executes the reader operation.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cmdText">The command text.</param>
        /// <param name="parameters">Options for controlling the operation.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A DbDataReader.</returns>
        public static DbDataReader ExecuteReader(this Database @this, string cmdText, DbParameter[] parameters, CommandType commandType)
        {
            return @this.ExecuteReader(cmdText, parameters, commandType, null);
        }
    }
}