// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType, SqlTransaction transaction)
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

            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="commandFactory">The command factory.</param>
    public static void SqlExecuteNonQuery(this Database @this, Action<SqlCommand> commandFactory)
    {
        var connection = (SqlConnection) @this.Connection;

        using (SqlCommand command = connection.CreateCommand())
        {
            commandFactory(command);

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText)
    {
        @this.SqlExecuteNonQuery(cmdText, null, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="transaction">The transaction.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, SqlTransaction transaction)
    {
        @this.SqlExecuteNonQuery(cmdText, null, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, CommandType commandType)
    {
        @this.SqlExecuteNonQuery(cmdText, null, commandType, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, CommandType commandType, SqlTransaction transaction)
    {
        @this.SqlExecuteNonQuery(cmdText, null, commandType, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, SqlParameter[] parameters)
    {
        @this.SqlExecuteNonQuery(cmdText, parameters, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="transaction">The transaction.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, SqlParameter[] parameters, SqlTransaction transaction)
    {
        @this.SqlExecuteNonQuery(cmdText, parameters, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute non query.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    public static void SqlExecuteNonQuery(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType)
    {
        @this.SqlExecuteNonQuery(cmdText, parameters, commandType, null);
    }
}