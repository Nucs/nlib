// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq.Expressions;

namespace Z.ExtensionMethods.EntityFramework
{
    public static class IDbSetExtension
    {
        /// <summary>
        ///     Adds or updates entities by key when SaveChanges is called. Equivalent to an "upsert" operation from database
        ///     terminology. This method can useful when seeding data using Migrations.
        /// </summary>
        /// <remarks>
        ///     When the <paramref name="set" /> parameter is a custom or fake IDbSet implementation, this method will
        ///     attempt to locate and invoke a public, instance method with the same signature as this extension method.
        /// </remarks>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <param name="set">.</param>
        /// <param name="entities">The entities to add or update.</param>
        public static void AddOrUpdateExtension<TEntity>(this IDbSet<TEntity> set, params TEntity[] entities) where TEntity : class
        {
            set.AddOrUpdate(entities);
        }

        /// <summary>
        ///     Adds or updates entities by a custom identification expression when SaveChanges is called. Equivalent to an
        ///     "upsert" operation from database terminology. This method can useful when seeding data using Migrations.
        /// </summary>
        /// <remarks>
        ///     When the <paramref name="set" /> parameter is a custom or fake IDbSet implementation, this method will
        ///     attempt to locate and invoke a public, instance method with the same signature as this extension method.
        /// </remarks>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <param name="set">.</param>
        /// <param name="identifierExpression">
        ///     An expression specifying the properties that should be used when determining
        ///     whether an Add or Update operation should be performed.
        /// </param>
        /// <param name="entities">The entities to add or update.</param>
        public static void AddOrUpdateExtension<TEntity>(this IDbSet<TEntity> set, Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities) where TEntity : class
        {
            set.AddOrUpdate(identifierExpression, entities);
        }
    }
}