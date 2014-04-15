// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using Z.ExtensionMethods;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType, SqlTransaction transaction)
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
                return reader.ToExpandoObjects();
            }
        }
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="commandFactory">The command factory.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, Action<SqlCommand> commandFactory)
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
                return reader.ToExpandoObjects();
            }
        }
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, null, CommandType.Text, null);
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, SqlTransaction transaction)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, null, CommandType.Text, transaction);
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, CommandType commandType)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, null, commandType, null);
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, CommandType commandType, SqlTransaction transaction)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, null, commandType, transaction);
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, SqlParameter[] parameters)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, parameters, CommandType.Text, null);
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, SqlParameter[] parameters, SqlTransaction transaction)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, parameters, CommandType.Text, transaction);
    }

    /// <summary>
    ///     Enumerates SQL execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process SQL execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> SqlExecuteExpandoObjects(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType)
    {
        return @this.SqlExecuteExpandoObjects(cmdText, parameters, commandType, null);
    }
}