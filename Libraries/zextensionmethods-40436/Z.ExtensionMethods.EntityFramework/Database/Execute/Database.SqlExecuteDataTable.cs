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
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType, SqlTransaction transaction)
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

            var ds = new DataSet();
            using (var sqlDataAdapter = new SqlDataAdapter(command))
            {
                sqlDataAdapter.Fill(ds);
            }

            return ds.Tables[0];
        }
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="commandFactory">The command factory.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, Action<SqlCommand> commandFactory)
    {
        var connection = (SqlConnection) @this.Connection;

        using (SqlCommand command = connection.CreateCommand())
        {
            commandFactory(command);

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var ds = new DataSet();
            using (var sqlDataAdapter = new SqlDataAdapter(command))
            {
                sqlDataAdapter.Fill(ds);
            }

            return ds.Tables[0];
        }
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText)
    {
        return @this.SqlExecuteDataTable(cmdText, null, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, SqlTransaction transaction)
    {
        return @this.SqlExecuteDataTable(cmdText, null, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, CommandType commandType)
    {
        return @this.SqlExecuteDataTable(cmdText, null, commandType, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, CommandType commandType, SqlTransaction transaction)
    {
        return @this.SqlExecuteDataTable(cmdText, null, commandType, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, SqlParameter[] parameters)
    {
        return @this.SqlExecuteDataTable(cmdText, parameters, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, SqlParameter[] parameters, SqlTransaction transaction)
    {
        return @this.SqlExecuteDataTable(cmdText, parameters, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute data table.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>A DataTable.</returns>
    public static DataTable SqlExecuteDataTable(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType)
    {
        return @this.SqlExecuteDataTable(cmdText, parameters, commandType, null);
    }
}