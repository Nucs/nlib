// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

public static partial class DbContexttExtension
{
    /// <summary>
    ///     A DbContext extension method that gets object context.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The object context.</returns>
    public static ObjectContext GetObjectContext(this DbContext @this)
    {
        return ((IObjectContextAdapter) @this).ObjectContext;
    }
}