// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Xml;
using System.Xml.Linq;

public static partial class Extension
{
    /// <summary>
    ///     A DbContext extension method that gets model x coordinate document.
    /// </summary>
    /// <param name="db">The db to act on.</param>
    /// <returns>The model x coordinate document.</returns>
    public static XDocument GetModelXDocument(this DbContext db)
    {
        XDocument doc;
        using (var memoryStream = new MemoryStream())
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings {Indent = true}))
            {
                EdmxWriter.WriteEdmx(db, xmlWriter);
            }

            memoryStream.Position = 0;

            doc = XDocument.Load(memoryStream);
        }
        return doc;
    }
}