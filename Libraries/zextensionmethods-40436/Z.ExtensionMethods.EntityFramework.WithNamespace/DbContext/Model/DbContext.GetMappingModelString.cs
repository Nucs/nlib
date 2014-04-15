// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System;
using System.Data.Entity;
using System.IO;

namespace Z.ExtensionMethods.EntityFramework
{
    public static partial class DbContexttExtension
    {
        /// <summary>
        ///     A DbContext extension method that gets mapping model string.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The mapping model string.</returns>
        public static string GetMappingModelString(this DbContext @this)
        {
            string model = string.Concat(@this.GetModelName(), ".msl");

            using (Stream stream = @this.GetType().Assembly.GetManifestResourceStream(model))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                throw new Exception("Stream not found.");
            }
        }
    }
}