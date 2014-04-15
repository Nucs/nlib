// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Xml;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType, SqlTransaction transaction)
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

            return command.ExecuteXmlReader();
        }
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="commandFactory">The command factory.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, Action<SqlCommand> commandFactory)
    {
        var connection = (SqlConnection) @this.Connection;

        using (SqlCommand command = connection.CreateCommand())
        {
            commandFactory(command);

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            return command.ExecuteXmlReader();
        }
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText)
    {
        return @this.SqlExecuteXmlReader(cmdText, null, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, SqlTransaction transaction)
    {
        return @this.SqlExecuteXmlReader(cmdText, null, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, CommandType commandType)
    {
        return @this.SqlExecuteXmlReader(cmdText, null, commandType, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, CommandType commandType, SqlTransaction transaction)
    {
        return @this.SqlExecuteXmlReader(cmdText, null, commandType, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, SqlParameter[] parameters)
    {
        return @this.SqlExecuteXmlReader(cmdText, parameters, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, SqlParameter[] parameters, SqlTransaction transaction)
    {
        return @this.SqlExecuteXmlReader(cmdText, parameters, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that SQL execute XML reader.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>An XmlReader.</returns>
    public static XmlReader SqlExecuteXmlReader(this Database @this, string cmdText, SqlParameter[] parameters, CommandType commandType)
    {
        return @this.SqlExecuteXmlReader(cmdText, parameters, commandType, null);
    }
}