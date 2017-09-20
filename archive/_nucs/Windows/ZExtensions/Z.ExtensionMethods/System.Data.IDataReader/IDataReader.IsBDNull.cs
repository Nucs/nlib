// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;

public static partial class IDataReaderExtension
{
    /// <summary>
    ///     An IDataReader extension method that query if '@this' is database null.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The name.</param>
    /// <returns>true if database null, false if not.</returns>
    public static bool IsDBNull(this IDataReader @this, string name)
    {
        return @this.IsDBNull(@this.GetOrdinal(name));
    }
}