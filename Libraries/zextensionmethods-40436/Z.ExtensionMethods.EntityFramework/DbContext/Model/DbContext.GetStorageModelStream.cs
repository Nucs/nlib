// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.IO;

public static partial class DbContexttExtension
{
    /// <summary>
    ///     A DbContext extension method that gets storage model stream.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The storage model stream.</returns>
    public static Stream GetStorageModelStream(this DbContext @this)
    {
        string connectionString = @this.Database.GetEntityConnection().ConnectionString;
        int end = connectionString.IndexOf(".ssdl") + 4;

        if (end == -1)
        {
            return null;
        }

        int start = connectionString.Substring(0, end).LastIndexOf("/") + 1;
        string modelName = connectionString.Substring(start, end - start + 1);

        return @this.GetType().Assembly.GetManifestResourceStream(modelName);
    }
}