// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Z.EntityFramework.Model;

public static partial class DbContexttExtension
{
    /// <summary>
    ///     A DbContext extension method that gets a model.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The model.</returns>
    public static Model GetModel(this DbContext @this)
    {
        ObjectContext objectContext = ((IObjectContextAdapter) @this).ObjectContext;
        MetadataWorkspace metadataWorkspace = objectContext.MetadataWorkspace;

        EntityContainer entityContainer;
        if (metadataWorkspace.TryGetEntityContainer("CodeFirstDatabase", true, DataSpace.SSpace, out entityContainer))
        {
            return @this.GetCodeFirstModel();
        }
        else
        {
            return @this.GetDatabaseFirstModel();
        }
    }
}