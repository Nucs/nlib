// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.IO;
using System.Xml.Serialization;

namespace Z.ExtensionMethods
{
    internal static class StringExtension
    {
        /// <summary>
        ///     A string extension method that deserialize XML.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        internal static T DeserializeXml<T>(this string @this)
        {
            var x = new XmlSerializer(typeof (T));
            var r = new StringReader(@this);

            return (T) x.Deserialize(r);
        }
    }
}