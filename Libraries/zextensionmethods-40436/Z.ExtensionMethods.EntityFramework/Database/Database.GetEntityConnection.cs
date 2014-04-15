// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Reflection;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that gets entity connection.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The entity connection.</returns>
    public static EntityConnection GetEntityConnection(this Database @this)
    {
        object internalContext = @this.GetType().GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@this);
        MethodInfo getObjectContext = internalContext.GetType().GetMethod("GetObjectContextWithoutDatabaseInitialization", BindingFlags.Public | BindingFlags.Instance);
        var objectContext = (ObjectContext) getObjectContext.Invoke(internalContext, null);
        DbConnection entityConnection = objectContext.Connection;

        return (EntityConnection) entityConnection;
    }
}