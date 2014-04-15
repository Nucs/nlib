// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Reflection;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DatabaseExtension
    {
        /// <summary>
        ///     A Database extension method that gets entity transaction.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The entity transaction.</returns>
        public static EntityTransaction GetEntityTransaction(this Database @this)
        {
            EntityConnection entityConnection = @this.GetEntityConnection();
            object entityTransaction = entityConnection.GetType().GetField("_currentTransaction", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entityConnection);

            return (EntityTransaction) entityTransaction;
        }
    }
}