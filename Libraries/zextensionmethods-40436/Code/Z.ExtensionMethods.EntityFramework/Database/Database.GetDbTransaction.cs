// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Common;
using System.Data.Entity;
using System.Reflection;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that gets database transaction.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The database transaction.</returns>
    public static DbTransaction GetDbTransaction(this Database @this)
    {
        object entityTransaction = @this.GetEntityTransaction();

        object transaction = entityTransaction.GetType().GetField("_storeTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entityTransaction);

        return (DbTransaction) transaction;
    }
}