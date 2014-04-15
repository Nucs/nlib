// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using Z.ExtensionMethods;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction)
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

            using (IDataReader reader = command.ExecuteReader())
            {
                return reader.ToExpandoObjects();
            }
        }
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="commandFactory">The command factory.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, Action<DbCommand> commandFactory)
    {
        DbConnection connection = @this.Connection;

        using (DbCommand command = connection.CreateCommand())
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
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText)
    {
        return @this.ExecuteExpandoObjects(cmdText, null, CommandType.Text, null);
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, DbTransaction transaction)
    {
        return @this.ExecuteExpandoObjects(cmdText, null, CommandType.Text, transaction);
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, CommandType commandType)
    {
        return @this.ExecuteExpandoObjects(cmdText, null, commandType, null);
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, CommandType commandType, DbTransaction transaction)
    {
        return @this.ExecuteExpandoObjects(cmdText, null, commandType, transaction);
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, DbParameter[] parameters)
    {
        return @this.ExecuteExpandoObjects(cmdText, parameters, CommandType.Text, null);
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, DbParameter[] parameters, DbTransaction transaction)
    {
        return @this.ExecuteExpandoObjects(cmdText, parameters, CommandType.Text, transaction);
    }

    /// <summary>
    ///     Enumerates execute expando objects in this collection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>
    ///     An enumerator that allows foreach to be used to process execute expando objects in this collection.
    /// </returns>
    public static IEnumerable<dynamic> ExecuteExpandoObjects(this Database @this, string cmdText, DbParameter[] parameters, CommandType commandType)
    {
        return @this.ExecuteExpandoObjects(cmdText, parameters, commandType, null);
    }
}