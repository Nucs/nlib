// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Reflection;

public static partial class DatabaseExtension
{
    /// <summary>
    ///     A Database extension method that gets database context.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The database context.</returns>
    public static DbContext GetDbContext(this Database @this)
    {
        object internalContext = @this.GetType().GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@this);
        return (DbContext) internalContext.GetType().BaseType.GetField("_owner", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(internalContext);
    }
}