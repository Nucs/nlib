// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using Z.EntityFramework.Model;
using Z.ExtensionMethods;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction) where T : new()
    {
        Model model = @this.GetDbContext().GetModel();
        ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == typeof (T).Name);

        if (entity == null)
        {
            throw new Exception(string.Format("Entity:{0} not found in DbContext.", typeof (T).Name));
        }

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
                reader.Read();
                return reader.ToEntity<T>(entity);
            }
        }
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="commandFactory">The command factory.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, Action<DbCommand> commandFactory) where T : new()
    {
        Model model = @this.GetDbContext().GetModel();
        ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == typeof (T).Name);

        if (entity == null)
        {
            throw new Exception(string.Format("Entity:{0} not found in DbContext.", typeof (T).Name));
        }

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
                reader.Read();
                return reader.ToEntity<T>(entity);
            }
        }
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, null, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, DbTransaction transaction) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, null, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, CommandType commandType) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, null, commandType, null);
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, CommandType commandType, DbTransaction transaction) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, null, commandType, transaction);
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, DbParameter[] parameters) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, parameters, CommandType.Text, null);
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, DbParameter[] parameters, DbTransaction transaction) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, parameters, CommandType.Text, transaction);
    }

    /// <summary>
    ///     A Database extension method that executes the entity with mapping operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="cmdText">The command text.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>A T.</returns>
    public static T ExecuteEntityWithMapping<T>(this Database @this, string cmdText, DbParameter[] parameters, CommandType commandType) where T : new()
    {
        return @this.ExecuteEntityWithMapping<T>(cmdText, parameters, commandType, null);
    }
}